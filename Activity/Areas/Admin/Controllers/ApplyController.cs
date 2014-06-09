using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;

namespace Activity.Areas.Admin.Controllers
{
	[Authorize(Users = "admin")]
    public class ApplyController : Controller
    {
		private SiteService siteService = new SiteService();

        //
        // GET: /Admin/Apply/

        public ActionResult Applies(int id)
        {
			var applies = siteService.GetApplies(id).ToList();

            return View(applies);
        }

    }
}
