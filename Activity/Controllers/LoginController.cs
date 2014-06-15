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

                var already = active.Applies.Where(m => m.Backup == "N").Count();

                var status = "";

                if (active.IsVolunteerFirst)
                {
                    if (active.Applies.Where(m => m.Backup == "V").Count() < active.VolunteerCount)
                    {
                        status += "<option value='V'>志愿者报名</option>";
                    }
                    else
                    {
                        if (already >= active.People)
                        {
                            if (active.Applies.Where(m => m.Backup == "Y").Count() < (int)(active.People * 0.15))
                            {
                                status += "<option value='Y'>候选报名</option>";
                            }
                        }
                        else
                        {
                            status += "<option value='N'>活动报名</option>";
                        }
                    }
                }
                else
                {
                    if (already >= active.People)
                    {
                        if (active.Applies.Where(m => m.Backup == "Y").Count() < (int)(active.People * 0.15))
                        {
                            status += "<option value='Y'>候选报名</option>";
                        }
                    }
                    else
                    {
                        status += "<option value='N'>活动报名</option>";
                    }

                    if (active.Applies.Where(m => m.Backup == "V").Count() < active.VolunteerCount)
                    {
                        status += "<option value='V'>志愿者报名</option>";
                    }
                }

                var u = new
                    {
                        UserID = user.UserID,
                        Contact = user.Contact,
                        Remind = active.Remind,
                        Option = status,
                        NeedPeople = "<span style='color: red;'>" + (active.IsVolunteerFirst ? "该活动优先接受志愿者报名. " : "") + "报名情况：</span>活动人员:" + active.People + "人,已报名" + already + "人; 志愿者:" + active.VolunteerCount + "名,已报名" + active.Applies.Where(mbox => mbox.Backup == "V").Count() + "人"
                    };

                return Json(u, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect("/");
        }
    }
}
