using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;

namespace Activity.Areas.Account.Controllers
{
	[Authorize]
    public class ProfileController : Controller
    {
		private MembershipService membershipService = new MembershipService();
        //
        // GET: /Account/Profile/

        public ActionResult Index()
        {
			var user = membershipService.GetUser(User.Identity.Name);

			return View(user);
        }

		[HttpPost]
		public ActionResult Index(User user)
		{
			user.UserID = User.Identity.Name;
			var result = membershipService.EditProfile(user);

			return Json(result);
		}

		public ActionResult Safe()
		{
			var userID = User.Identity.Name;
			var user = membershipService.GetUser(userID);

			return View(user);
		}

		[HttpPost]
		public ActionResult Safe(User user)
		{
			var result = membershipService.ChangePassword(User.Identity.Name, user.Password);
			membershipService.Save();

			return Json(result);
		}
    }
}
