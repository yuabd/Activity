using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Activity.Service;
using Activity.Models.Others;
using Activity.Models;
using System.Web.Script.Serialization;

namespace Activity.Areas.Admin.Controllers
{
	[Authorize(Users = "admin")]
    public class SettingsController : Controller
    {
		private MembershipService membershipService = new MembershipService();
		private SiteService siteService = new SiteService();
        //
        // GET: /Admin/Settings/

		public ActionResult TagsList(int? page)
		{
			var tags = siteService.GetTags().Where(m => m.IsPublic == "Y").ToList();
			var model = new Paginated<ActiveTag>(tags, page ?? 1, 10);

			return View(model);
		}

		public ActionResult AddTag(string tag)
		{
			var result = siteService.AddTag(tag);

			return Json(result);
		}

		public ActionResult Remove(string id)
		{
			var result = siteService.Remove(id);

			return Json(result);
		}

		public ActionResult GetUser(string id)
		{
			var user = membershipService.GetUser(id);
			var javaScriptSerializer = new JavaScriptSerializer();

			return Json(new
			{
				UserID = user.UserID,
				Gender = user.Gender,
				City = user.City,
				Email = user.Email
			}, JsonRequestBehavior.AllowGet);
		}

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult HotTags()
		{
			var Tags = siteService.GetTags().Where(m => m.IsPublic == "Y").ToList();
			string str = "";
			foreach (var item in Tags)
			{
				str += item.Tag + " ";
			}

			ViewBag.Tags = str;

			return View();
		}

		public ActionResult HotTagsJson(string tags)
		{
			return Json(siteService.AddTag(tags));
		}

		public ActionResult Company()
		{
			//SiteSettings siteSettings = new SiteSettings();
            var site = new SysConfigHelper().GetSysConfig();

            return View(site);
		}

		[HttpPost]
        [ValidateInput(false)]
        public ActionResult Company(SysConfig sysConfig)
		{
            BaseObject obj = new SysConfigHelper().UpdateSysConfig(sysConfig);

            //var xml = XDocument.Load(Server.MapPath("~/SiteSettings.xml"));
            //XAttribute field;

            //field = (from m in xml.Descendants("companyName") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.CompanyName);
            //field = (from m in xml.Descendants("companyNameFull") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.CompanyNameFull);
            //field = (from m in xml.Descendants("companyWebsite") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.CompanyWebsite);
            ////field = (from m in xml.Descendants("companyEmail") select m.Attribute("value")).SingleOrDefault();
            ////field.SetValue(siteSettings.CompanyEmail);
            ////field = (from m in xml.Descendants("companyEmailAuto") select m.Attribute("value")).SingleOrDefault();
            ////field.SetValue(siteSettings.CompanyEmailAuto);
            //field = (from m in xml.Descendants("companyPhoneNo") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.CompanyPhoneNo);
            //field = (from m in xml.Descendants("showName") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.ShowName);
            //field = (from m in xml.Descendants("Notice") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.Notice);
            //field = (from m in xml.Descendants("IsShowBox") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(siteSettings.IsShowBox);
            //field = (from m in xml.Descendants("HomeBoxUrl") select m.Attribute("value")).SingleOrDefault();
            //field.SetValue(string.IsNullOrEmpty(siteSettings.HomeBoxUrl) ? "" : siteSettings.HomeBoxUrl);

            //xml.Save(Server.MapPath("~/SiteSettings.xml"));
            //obj.Tag = 1;

			return Json(obj);
		}

		public ActionResult Users()
		{
			var users = membershipService.GetUsers();
			return View(users);
		}

		[HttpPost]
		public ActionResult Users(User user, string _userID, string[] userRole)
		{
			if (string.IsNullOrEmpty(_userID))
				membershipService.InsertUser(user,userRole);
			else
			{
				user.UserID = _userID;
				membershipService.UpdateUser(user,userRole);
			}

			membershipService.Save();
			return RedirectToAction("Users");
		}

		public JsonResult GetUserRoles(string id)
		{
			string[] rolelist = membershipService.GetUserRoles(id).Split(',');

			return Json(rolelist, JsonRequestBehavior.AllowGet);
		}

		public JsonResult CheckUser(string userID)
		{
			var role = membershipService.GetUser(userID);
			return Json(role == null, JsonRequestBehavior.AllowGet);
		}

		public ActionResult DeleteUser(string id)
		{
			membershipService.DeleteUser(id);
			membershipService.Save();

			return RedirectToAction("Users");
		}

		public ActionResult Roles()
		{
			var roles = membershipService.GetUserRoles();
			return View(roles);
		}

		[HttpPost]
		public ActionResult Roles(UserRole userRole, string oldRole)
		{
			if (!string.IsNullOrEmpty(userRole.RoleID))
			{
				if (string.IsNullOrEmpty(oldRole))
					membershipService.InsertUserRole(userRole);
				else
					membershipService.UpdateUserRole(userRole, oldRole);

				membershipService.Save();
			}

			return RedirectToAction("Roles");
		}

		public ActionResult DeleteRole(string id)
		{
			membershipService.DeleteUserRole(id);
			membershipService.Save();
			return RedirectToAction("Roles");
		}

		public JsonResult CheckRole(string roleID)
		{
			var role = membershipService.GetUserRole(roleID);
			return Json(role == null, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Password()
		{
			var userID = User.Identity.Name;
			var user = membershipService.GetUser(userID);

			return View(user);
		}

		[HttpPost]
		public ActionResult Password(User user)
		{
			if (ModelState.IsValid)
			{
				membershipService.ChangePassword(user.UserID, user.Password);
				membershipService.Save();

				return View(user);
			}
			else
			{
				return View(user);
			}
			
		}

		public ActionResult Profiles(string id)
		{
			var user = membershipService.GetUser(id);

			return View(user);
		}
    }
}
