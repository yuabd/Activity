using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Activity.Helpers;
using Activity.Models;
using Activity.Models.Others;
using Activity.Service;

namespace Activity.Areas.Admin.Controllers
{
	//[Authorize(Roles = "Administrator")]
    public class BlogController : Controller
    {
		BlogService blogService = new BlogService();

        //
        // GET: /Admin/Blog/

		public ActionResult Index(int? page, int? categoryID)
		{
			var blogs = blogService.GetBlogs().ToList();

			if (categoryID != null && categoryID > 0)
			{
				blogs = blogs.Where(m => m.CategoryID == categoryID).ToList();
				ViewBag.Category = blogService.GetBlogCategory((int)categoryID);
			}

			var pblogs = new Paginated<Blog>(blogs, page ?? 1, 25);

			return View(pblogs);
		}

		public ActionResult Add(int? categoryID)
		{
			var blog = new Blog();
			blog.AuthorID = User.Identity.Name;
			blog.DateCreated = DateTime.Now;
			blog.IsPublic = true;
			blog.CategoryID = (int)(categoryID ?? 0);
			//IEnumerable<BlogTag> blogTags = null;

			//var model = new BlogViewModel(blog, blogTags);

			return View(blog);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Add(Blog blog)
		{
			if (ModelState.IsValid)
			{
				HttpPostedFileBase file = Request.Files["PictureFile"];

				//if (string.IsNullOrEmpty(blog.MetaKeywords))
				//{
				//    foreach (var tag in blogTags)
				//    {
				//        if (!string.IsNullOrEmpty(tag.Tag))
				//            blog.MetaKeywords += tag.Tag + ",";
				//    }
				//}

				blogService.InsertBlog(blog, file);
				blogService.Save();


				//blogService.SaveBlogTags(blog,blogTags);

				return RedirectToAction("Index", new { categoryID = blog.CategoryID });
			}
			else
			{
				//var model = new BlogViewModel(blog, blogTags);
				return View(blog);
			}
		}

		public ActionResult Edit(int id)
		{
			var blog = blogService.GetBlog(id);
			//var blogTags = blogService.GetBlogTags(id);

			//var model = new BlogViewModel(blog, blogTags);

			return View(blog);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(Blog blog)
		{
			if (ModelState.IsValid)
			{
				HttpPostedFileBase file = Request.Files["PictureFile"];

				//if (string.IsNullOrEmpty(blog.MetaKeywords))
				//{
				//    foreach (var tag in blogTags)
				//    {
				//        if (!string.IsNullOrEmpty(tag.Tag))
				//            blog.MetaKeywords += tag.Tag + ",";
				//    }
				//}

				blogService.UpdateBlog(blog, file);
				//blogService.SaveBlogTags(blog, blogTags);
				blogService.Save();

				return RedirectToAction("Index", new { categoryID = blog.CategoryID });
			}
			else
			{
				//var model = new BlogViewModel(blog, blogTags);
				return View(blog);
			}
		}

		public ActionResult Delete(int id)
		{
			var blog = blogService.GetBlog(id);
			blogService.DeleteBlog(blog);
			blogService.Save();

			return RedirectToAction("Index", new { categoryID = blog.CategoryID });
		}

		//
		// Comments

		//public ActionResult PendingComments()
		//{
		//    var blogs = blogService.GetBlogsWithPendingComments().ToList();
		//    return View(blogs);
		//}

		//// TODO: is this being used?
		//[HttpPost]
		//public string AddComment(BlogComment comment)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        blogService.InsertBlogComment(comment);
		//        blogService.Save();

		//        return "Thank you for your comment";
		//    }
		//    else
		//    {
		//        return "Error";
		//    }
		//}

		//public ActionResult ApproveComment(int id)
		//{
		//    blogService.ApproveBlogComment(id);
		//    return RedirectToAction("PendingComments");
		//}

		//public ActionResult DeleteComment(int id)
		//{
		//    blogService.DeleteBlogComment(id);
		//    blogService.Save();

		//    return RedirectToAction("PendingComments");
		//}

		//
		// GET: /Admin/Blog/Categories

		public ActionResult Categories()
		{
			var categories = blogService.GetBlogCategories().ToList();

			return View(categories);
		}

		[HttpPost]
		public ActionResult Categories(BlogCategory category)
		{
			if (ModelState.IsValid)
			{
				if (category.CategoryID > 0)
				{
					blogService.UpdateBlogCategory(category);
					blogService.Save();
				}
				else
				{
					blogService.InsertBlogCategory(category);
					blogService.Save();
				}

				return RedirectToAction("Categories");
			}
			else
			{
				return RedirectToAction("Categories");
			}
		}

		public ActionResult DeleteCategory(int id)
		{
			blogService.DeleteBlogCategory(id);
			blogService.Save();

			return RedirectToAction("Categories");
		}

		public ActionResult UploadPicture(HttpPostedFileBase filedata)
		{
			xheditorModel model = new xheditorModel();

			if (filedata.ContentLength > 0)
			{
				var fileName = filedata.FileName;
				IO.UploadImageFile(filedata.InputStream, "/Content/Pictures/Blog", fileName, 720);

				model.msg = "/Content/Pictures/Blog/" + fileName;
			}

			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			return this.Content(javaScriptSerializer.Serialize(model));
		}
    }
}
