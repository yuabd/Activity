using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Service;
using Activity.Models;

namespace Activity.Controllers
{
	public class NewsController : Controller
	{
		BlogService blogService = new BlogService();
		private VoteService voteService = new VoteService();
		//
		// GET: /News/

		public ActionResult Index(int id, int? page)
		{
			var blogs = blogService.GetBlogs().Where(m => m.CategoryID == id).ToList();
			var model = new Paginated<Blog>(blogs, page ?? 1, 10);
			ViewBag.Category = blogService.GetBlogCategory(id);

			return View(model);
		}

		public ActionResult Detail(int id, string linkUrl = "")
		{
			var blog = blogService.GetBlog(id);

			ViewBag.Discuss = voteService.GetDiscusses(id, Activity.Models.Others.DiscussType.News, linkUrl);
			ViewBag.linkUrl = linkUrl;

			blog.PageVisits += 1;
			blogService.Save();

			return View(blog);
		}
	}
}
