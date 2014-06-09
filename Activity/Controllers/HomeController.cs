using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Activity.Service;
using Activity.Models;
using Activity.Models.Others;

namespace Activity.Controllers
{
    public class HomeController : Controller
    {
        private SiteService siteService = new SiteService();
        private PictureService pictureService = new PictureService();
        private VoteService voteService = new VoteService();
        //
        // GET: /Home/

        public ActionResult Index(int? page)
        {
            var actives = siteService.GetActives().ToList();
            var model = new Paginated<Active>(actives, page ?? 1, 8);
            ViewBag.Photos = pictureService.GetPhotos().ToList();

            //var xml = XDocument.Load(Server.MapPath("~/XML.xml"));
            //XAttribute field;

            //field = (from m in xml.Descendants("visitor") select m.Attribute("value")).SingleOrDefault();
            //var result = (int.Parse(field.Value) + 1).ToString();
            //field.SetValue(result);
            //xml.Save(Server.MapPath("~/XML.xml"));

            new SysConfigHelper().AddVisit();

            ViewBag.Company = new SysConfigHelper().GetSysConfig();

            return View(model);
        }

        public ActionResult Good(int id)
        {
            var result = siteService.Good(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Categories(int? id, int? page, string tag, string date)
        {
            ViewBag.CategoryID = id;
            var actives = new List<Active>();
            if (id != null)
            {
                actives = siteService.GetActives().Where(m => m.CategoryID == id && m.IsPublic == true).ToList();
            }
            else
            {
                actives = siteService.GetActives().Where(m => m.IsPublic == true).ToList();
            }

            if (!string.IsNullOrEmpty(tag))
            {
                var tags = siteService.GetTagJoins(tag).Select(m => m.ActiveID).ToList();

                actives = actives.Where(m => tags.Contains(m.ActiveID)).ToList();
            }

            if (!string.IsNullOrEmpty(date))
            {
                switch (date)
                {
                    case "1":
                        actives = (from a in actives
                                   where
                                       DateTime.Now >= a.StartDate && DateTime.Now <= a.EndDate
                                   select a).ToList();
                        break;
                    case "2":
                        actives = (from a in actives
                                   where
                                      DateTime.Now.AddDays(1) >= a.StartDate && DateTime.Now.AddDays(1) <= a.EndDate
                                   select a).ToList();
                        break;
                    case "7":
                        actives =
                                actives.Where(m => m.StartDate <= DateTime.Now && m.EndDate >= DateTime.Now.AddDays(7)).ToList();
                        break;
                    case "30":
                        actives = (from a in actives
                                   where
                                      a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now.AddMonths(1)
                                   select a).ToList();
                        break;
                }
            }

            var model = new Paginated<Active>(actives, page ?? 1, 10);

            return View(model);
        }

        public ActionResult Detail(int id, string linkUrl = "")
        {
            var active = siteService.GetActive(id);
            ViewBag.Category = active.Category.CategoryName;
            ViewBag.CategoryID = active.Category.CategoryID;
            ViewBag.Applies = active.Applies.Where(m => m.Backup == "N").Take(20).ToList();
            ViewBag.Backup = active.Applies.Where(m => m.Backup == "Y").Take(20).ToList();
            ViewBag.Volunteer = active.Applies.Where(m => m.Backup == "V").Take(20).ToList();

            ViewBag.Discuss = voteService.GetDiscusses(id, Activity.Models.Others.DiscussType.Active, linkUrl);
            ViewBag.linkUrl = linkUrl;

            active.PageVisits += 1;
            siteService.Save();

            return View(active);
        }

        public ActionResult CheckActive(int id)
        {
            var obj = siteService.CheckActive(id);

            return Json(obj);
        }

        public ActionResult More(int id)
        {
            var active = siteService.GetActive(id);
            var applies = active.Applies.Where(m => m.Backup == "N").Skip(20).Select(m => m.Name).ToList();

            return Json(applies);
        }

        public ActionResult Apply(Apply apply)
        {
            apply.UserID = User.Identity.Name;
            apply.DateCreated = DateTime.Now;
            //apply.
            var result = siteService.InsertApply(apply);

            return Json(result);
        }

        public ActionResult Search(string keyword, int? page)
        {
            var actives = new List<Active>();
            if (!string.IsNullOrEmpty(keyword))
            {
                actives = siteService.Search(keyword).ToList();
            }

            //var model = new Paginated<Active>(actives, page ?? 1, 1);

            return View(actives);
        }

        public ActionResult Tags(int? page)
        {
            var tags = siteService.GetTags().ToList();
            var model = new Paginated<ActiveTag>(tags, page ?? 1, 60);

            return View(model);
        }

        public ActionResult VoteList(int? page)
        {
            var list = voteService.GetVoteList();
            var model = new Paginated<Vote>(list, page ?? 1, 15);

            return View(model);
        }

        public ActionResult Vote(int id)
        {
            var vote = voteService.GetVoteByID(id);
            var details = voteService.GetVoteDetailByVoteID(id);
            ViewBag.Details = details;
            var count = details.Sum(m => m.Count);
            ViewBag.Count = count;
            var isVoted = "N";
            if (User.Identity.IsAuthenticated)
            {
                var uV = voteService.GetUserVotes().FirstOrDefault(m => m.VoteID == id && m.UserID == User.Identity.Name);
                if (uV != null)
                {
                    isVoted = "Y";
                }
            }

            ViewBag.IsVoted = isVoted;

            return View(vote);
        }

        public ActionResult UserVote(UserVote userVote)
        {
            userVote.UserID = User.Identity.Name;
            var result = voteService.InsertUserVote(userVote);

            return Json(result);
        }

        public ActionResult SubmitDiscuss(Discuss discuss)
        {
            discuss.DateCreated = DateTime.Now;
            discuss.UserID = User.Identity.Name;
            var result = voteService.InsertDiscuss(discuss);

            return Json(result);
        }
    }
}
