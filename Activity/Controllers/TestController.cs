using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;

namespace Activity.Controllers
{
    public class TestController : Controller
    {
        SiteService sb = new SiteService();
        //
        // GET: /Test/

        public ActionResult Index()
        {
            var ss = sb.SendSms("15980751561", "djdjdjdjdj", "admin", 1);

            return View();
        }

    }
}
