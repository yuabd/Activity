using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;
using Activity.Models.Others;
using Activity.Helpers;

namespace Activity.Areas.Account.Controllers
{
	[Authorize]
    public class ActiveController : Controller
    {
		private SiteService siteService = new SiteService();
        //
        // GET: /Account/Active/

		public ActionResult Add()
		{
			var active = new Active();
			active.UserID = User.Identity.Name;
            active.ContactPeople = User.Identity.Name;
            active.ContactPerson = User.Identity.Name;
            var user = new MembershipService().GetUser(active.UserID);
            active.PersonPhone = user.Contact;

			return View(active);
		}

		[HttpPost]
		[ValidateInput(false)]
        public ActionResult Add(Active active)
		{
			active.DateCreated = DateTime.Now;
			
			var result = siteService.AddActivity(active);

			return Json(result);
		}

        public ActionResult VolunteerView(int id)
        {
            ViewBag.ID = id;
            var list = siteService.GetVolunteerByActive(id);

            return View(list);
        }

        public ActionResult VolunteerJson()
        {
            string volunteerType = Request["volunteerType"].UString();
            int volunteerPeople = Request["volunteerPeople"].Uint();
            int activeID = Request["ActiveID"].Uint();
            var obj = siteService.AddVolunteer(volunteerType, volunteerPeople, activeID);

            return Json(obj);
        }

        public ActionResult DeleteVolunteer(int? id)
        {
            var obj = siteService.DeleteVolunteer(id);

            return Json(obj);
        }

		public ActionResult Edit(int id)
		{
			var active = siteService.GetActive(id);
			//ViewBag.Account = "disabled";
			var tags = siteService.GetTagJoins(id).ToList();

			return View(active);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(Active active)
		{
			var result = siteService.UpdateActive(active);

			return Json(result);
		}

		public ActionResult MakeEnd(int id)
		{
			var active = siteService.GetActive(id);

			return View(active);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult MakeEnd(Active active)
		{
			BaseObject obj = new BaseObject();

			var a = siteService.GetActive(active.ActiveID);
			a.EndContent = active.EndContent;

			siteService.Save();

			obj.Tag = 1;

			return Json(obj);
		}

		public ActionResult Applies(int? page)
		{
			var applies = siteService.GetApplies(User.Identity.Name).ToList();
			var model = new Paginated<Apply>(applies, page ?? 1, 10);

			return View(model);
		}

		public DateTime date = DateTime.Now.Date;
        public ActionResult Index(int? page)
        {
			
			var actives = siteService.GetActives(User.Identity.Name).Where(m => m.StartDate <= date && m.EndDate >= date).ToList();

			var model = new Paginated<Active>(actives, page ?? 1, 10);

			return View(model);
        }

		public ActionResult NotStart(int? page)
		{
			var actives = siteService.GetActives(User.Identity.Name).Where(m => m.StartDate > date).ToList();

			var model = new Paginated<Active>(actives, page ?? 1, 10);

			return View(model);
		}

		public ActionResult EndActive(int? page)
		{
			var actives = siteService.GetActives(User.Identity.Name).Where(m => m.EndDate < date).ToList();

			var model = new Paginated<Active>(actives, page ?? 1, 10);

			return View(model);
		}

		public ActionResult Wait(int? page)
		{
			var actives = siteService.GetWaitActives(User.Identity.Name).ToList();

			var model = new Paginated<Active>(actives, page ?? 1, 10);

			return View(model);
		}

		public ActionResult ActiveApplies(int id, int? page)
		{
			var applies = siteService.GetApplies(id).OrderBy(m => m.Backup).ToList();
			var model = new Paginated<Apply>(applies, page ?? 1, 10);

			return View(model);
		}

		public ActionResult Cancel(int id)
		{
			var cancel = siteService.DelApply(id);

			return Json(cancel);
		}
    }
}
