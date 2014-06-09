using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;
using Activity.Models.ViewModels;

namespace Activity.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class VoteController : Controller
    {
        VoteService voteService = new VoteService();
        //
        // GET: /Admin/Vote/

        public ActionResult Index(int? page)
        {
            var votes = voteService.GetVoteList();
            var model = new Paginated<Vote>(votes, page ?? 1, 15);

            return View(model);
        }

        public ActionResult Add()
        {
            var vote = new Vote();
            vote.EndDate = null;
            return View(vote);
        }

        public ActionResult AddJson(Vote vote, string names)
        {
            vote.StartDate = DateTime.Now;
            vote.UserID = User.Identity.Name;
            var result = voteService.InsertVote(vote, names);

            return Json(result);
        }

        public ActionResult Edit(int id)
        {
            var result = voteService.GetVoteByID(id);
            var details = voteService.GetVoteDetailByVoteID(id);
            VoteViewModel de = new VoteViewModel();
            de.Vote = result;
            de.VoteDetails = details;
            //var names = "";
            //foreach (var item in details)
            //{
            //    names += item.Name + ";";
            //}
            //ViewBag.Names = details;

            return View(de);
        }

        public ActionResult EditJson(Vote vote, string names)
        {
            var result = voteService.UpdateVote(vote, names);

            return Json(result);
        }

        public ActionResult DeleteJson(int id)
        {
            var result = voteService.DeleteVote(id);

            return Json(result);
        }

        public ActionResult EditDetailView(int id)
        {
            var result = voteService.GetVoteDetailByID(id);

            return View(result);
        }

        public ActionResult EditDetailJson(VoteDetail voteDetail)
        {
            var result = voteService.UpdateVoteDetail(voteDetail);

            return Json(result);
        }

    }
}
