using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Activity.Models.ViewModels;
using Activity.Service;
using System.Web.Security;
using Activity.Models;

namespace Activity.Controllers
{
    public class LoginController : Controller
    {
		private MembershipService membershipService = new MembershipService();
        private SiteService siteService = new SiteService();
        //
        // GET: /Login/

		public ActionResult Index(string returnUrl)
        {
			LoginViewModel model = new LoginViewModel();

			return View(model);
        }

		[HttpPost]
		public ActionResult Index(LoginViewModel model, string returnUrl)
		{
			var loginMessage = membershipService.Login(model.UserID, model.Password, model.RememberMe);

			return Json(loginMessage);
		}

		public ActionResult Register()
		{
			var user = new User();

			return View(user);
		}

		[HttpPost]
		public ActionResult Register(User user)
		{
			var ps = user.Password;
			var result = membershipService.Register(user);
			membershipService.Login(user.UserID, ps, true);

			return Json(result);
		}

		public ActionResult UserInfo(int? id)
		{
			var user = membershipService.GetUser(User.Identity.Name);
            var active = new Active();
            if (id != null)
            {
                active = siteService.GetActive((int)id);
            }

            if (active.Applies.Where(m => m.Backup == "N").Count() > active.People)
            {

            }

			var u = new
				{
					UserID = user.UserID,
					Contact = user.Contact,
                    Remind = active.Remind
				};

			return Json(u, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Abandon();

			return Redirect("/");
		}
    }
}
