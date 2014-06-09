using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Activity.Helpers;
using Activity.Models;
using System.Web.Mvc;

namespace Activity.Service
{
	public class BlogService
	{
		private SiteDataContext db = new SiteDataContext();

		// Blog

		public void InsertBlog(Blog blog, HttpPostedFileBase file)
		{
			if (string.IsNullOrEmpty(blog.PageTitle))
			{
				blog.PageTitle = blog.BlogTitle;
			}

			db.Blogs.Add(blog);
			Save();

			// add slug after (depends on ID)
			if (string.IsNullOrEmpty(blog.Slug))
			{
				blog.Slug = Utilities.GenerateSlug(blog.BlogTitle, 50) + "-" + blog.ID.ToString();
			}

			// file
			if (file.ContentLength > 0)
			{
				UploadBlogPicture(blog, file);
			}
		}

		public Blog GetBlog(int blogID)
		{
			return db.Blogs.Find(blogID);
		}

		public void UpdateBlog(Blog blog, HttpPostedFileBase file)
		{
			var b = GetBlog(blog.ID);

			// data
			if (string.IsNullOrEmpty(blog.Slug))
			{
				b.Slug = Utilities.GenerateSlug(blog.BlogTitle, 50) + "-" + blog.ID.ToString();
			}

			b.CategoryID = blog.CategoryID;
			b.BlogTitle = blog.BlogTitle;
			b.BlogContent = blog.BlogContent;
			b.DateCreated = blog.DateCreated;
			b.PageTitle = blog.PageTitle;
			b.MetaDescription = blog.MetaDescription;
			b.MetaKeywords = blog.MetaKeywords;
			b.Slug = blog.Slug;
			b.IsPublic = blog.IsPublic;

			// file
			if (file.ContentLength > 0)
			{
				UploadBlogPicture(b, file);
			}
		}

		public void DeleteBlog(Blog blog)
		{
			
			db.Blogs.Remove(blog);

			DeleteBlogPicture(blog);
		}

		public Blog GetBlog(string slug)
		{
			return db.Blogs.FirstOrDefault(m => m.Slug == slug);
		}

		public Blog GetLastBlog()
		{
			return db.Blogs.OrderByDescending(m => m.DateCreated).Where(m => m.IsPublic == true).Take(1).SingleOrDefault();
		}

		public IQueryable<Blog> GetBlogs()
		{
			return db.Blogs.OrderByDescending(b => b.DateCreated);
		}

		public IQueryable<Blog> GetBlogs(string languageID)
		{
			return GetBlogs().Where(m => m.BlogCategory.LanguageID == languageID);
		}

		public IQueryable<Blog> GetBlogsByCategory(string slug)
		{
			var l = from b in db.Blogs
					join bc in db.BlogCategories on b.CategoryID equals bc.CategoryID
					where bc.Slug == slug
					orderby b.DateCreated descending
					select b;

			return l;
		}

		//public IQueryable<Blog> GetBlogsByTag(string tag)
		//{
		//    var l = from b in db.Blogs
		//            join bc in db.BlogTags on b.BlogID equals bc.BlogID
		//            where bc.Tag == tag
		//            orderby b.DateCreated descending
		//            select b;

		//    return l;
		//}

		//public IQueryable<Blog> GetBlogsByTag(string tag, string languageID)
		//{
		//    var l = from b in db.Blogs
		//            join bc in db.BlogTags on b.BlogID equals bc.BlogID
		//            where bc.Tag == tag && b.BlogCategory.LanguageID == languageID
		//            orderby b.DateCreated descending
		//            select b;

		//    return l;
		//}

		public IQueryable<Blog> GetBlogsByArchive(string year, string month, string type)
		{
			if (string.IsNullOrEmpty(month))
				month = "January";
			DateTime fromTime = Convert.ToDateTime(year + "/" + month);
			DateTime toTime = fromTime;
			if (type == "year")
				toTime = fromTime.AddYears(1);
			else if (type == "month")
				toTime = fromTime.AddMonths(1);

			return db.Blogs.Where(b => b.DateCreated >= fromTime && b.DateCreated <= toTime).OrderByDescending(b => b.DateCreated);
		}

		public IQueryable<Blog> GetBlogsByArchive(string year, string month, string type, string languageID)
		{
			return GetBlogsByArchive(year, month, type).Where(m => m.BlogCategory.LanguageID == languageID);
		}

		// Blog Comment

		//public void InsertBlogComment(BlogComment blogComment)
		//{
		//    blogComment.IsPublic = false;
		//    blogComment.DateCreated = DateTime.Now;
		//    db.BlogComments.Add(blogComment);
		//}

		//public void UpdateBlogComment(BlogComment blogComment)
		//{
		//    var bf = GetBlogComment(blogComment.CommentID);
		//    bf.Message = blogComment.Message;
		//}

		//public void DeleteBlogComment(int commentID)
		//{
		//    var c = GetBlogComment(commentID);
		//    db.BlogComments.Remove(c);
		//}

		//public BlogComment GetBlogComment(int commentID)
		//{
		//    return db.BlogComments.Find(commentID);
		//}

		//public IQueryable<BlogComment> GetBlogComments(int blogID)
		//{
		//    return db.BlogComments.Where(p => p.BlogID == blogID);
		//}

		//public void ApproveBlogComment(int commentID)
		//{
		//    var c = GetBlogComment(commentID);
		//    c.IsPublic = true;
		//    Save();

		//    // notify posting user

		//    var subject = "Your comment in our blog has been approved";

		//    var message = string.Format("<p>Hi {0},</p>" +
		//        "<p>Your message has been approved:</p>" +
		//        "{1}" +
		//        "<p>Blog: <a href='/Blog/Post/{2}'>{3}</a></p>",
		//        c.Name,
		//        c.Message,
		//        c.Blog.Slug,
		//        c.Blog.BlogTitle
		//        );

		//    Hammer.Common.Models.MailBag mailBag = new Hammer.Common.Models.MailBag();

		//    mailBag.ToMailAddress = c.Email;
		//    mailBag.Subject = subject;
		//    mailBag.Message = message;
		//    mailBag.Send(true);

		//    // notify everybody else

		//    var others = from bc in db.BlogComments
		//                 where bc.BlogID == c.BlogID && bc.Email != c.Email
		//                 select bc;

		//    foreach (var item in others)
		//    {
		//        subject = string.Format("New comment on: {0}", c.Blog.BlogTitle);

		//        message = string.Format("<p>Hi {0},</p>" +
		//            "<p>A new message has been posted by <b>{1}</b> on <a href='/Blog/Post/{2}'>{3}</a>:</p>" +
		//            "{4}",
		//            item.Name,
		//            c.Name,
		//            c.Blog.Slug,
		//            c.Blog.BlogTitle,
		//            c.Message
		//            );

		//        mailBag.ToMailAddress = item.Email;
		//        mailBag.Subject = subject;
		//        mailBag.Message = message;
		//        mailBag.Send(false);
		//    }
		//}

		//public IQueryable<Blog> GetBlogsWithPendingComments()
		//{
		//    var r = from b in db.Blogs
		//            where db.BlogComments.Any(c => c.IsPublic == false && c.BlogID == b.BlogID)
		//            select b;

		//    return r;
		//}

		// Blog Category

		public void InsertBlogCategory(BlogCategory blogCategory)
		{
			if (string.IsNullOrEmpty(blogCategory.PageTitle))
			{
				blogCategory.PageTitle = blogCategory.CategoryName;
			}

			if (string.IsNullOrEmpty(blogCategory.Slug))
			{
				blogCategory.Slug = Utilities.GenerateSlug(blogCategory.CategoryName, 50);
			}

			db.BlogCategories.Add(blogCategory);
		}

		public BlogCategory GetBlogCategory(int id)
		{
			return db.BlogCategories.Find(id);
		}

		public void UpdateBlogCategory(BlogCategory blogCategory)
		{
			var bc = GetBlogCategory(blogCategory.CategoryID);

			if (string.IsNullOrEmpty(blogCategory.Slug))
			{
				bc.Slug = Utilities.GenerateSlug(blogCategory.CategoryName, 50);
			}

			bc.CategoryName = blogCategory.CategoryName;
			bc.PageTitle = blogCategory.PageTitle;
			bc.MetaDescription = blogCategory.MetaDescription;
			bc.MetaKeywords = blogCategory.MetaKeywords;
			bc.LanguageID = blogCategory.LanguageID;	// used for multilanguage sites
			bc.SortOrder = blogCategory.SortOrder;
		}

		public void DeleteBlogCategory(int categoryID)
		{
			var bc = GetBlogCategory(categoryID);
			db.BlogCategories.Remove(bc);
		}

		public IQueryable<BlogCategory> GetBlogCategories()
		{
			return db.BlogCategories.OrderBy(m => m.SortOrder);
		}

		public IQueryable<BlogCategory> GetBlogCategories(string languageID)
		{
			return GetBlogCategories().Where(m => m.LanguageID == languageID);
		}

		//Blog Tag

		//public IQueryable<String> GetPopularTags()
		//{
		//    var pt = from p in db.BlogTags
		//             group p by new { p.Tag } into t
		//             orderby t.Count() descending
		//             select t.Key.Tag;

		//    return pt;
		//}

		//public IQueryable<String> GetPopularTags(string languageID)
		//{
		//    var pt = from p in db.BlogTags
		//             where p.Blog.BlogCategory.LanguageID==languageID
		//             group p by new { p.Tag } into t
		//             orderby t.Count() descending
		//             select t.Key.Tag;

		//    return pt;
		//}

		//public IQueryable<BlogTag> GetBlogTags(int blogID)
		//{
		//    return db.BlogTags.Where(b => b.BlogID == blogID);
		//}

		//public void SaveBlogTags(Blog blog, IEnumerable<BlogTag> blogTags)
		//{
		//    var bt = GetBlogTags(blog.BlogID);

		//    foreach (BlogTag tag in bt)
		//    {
		//        DeleteBlogTag(tag);
		//    }

		//    foreach (BlogTag tag in blogTags)
		//    {
		//        tag.BlogID = blog.BlogID;
		//        if (!string.IsNullOrEmpty(tag.Tag))
		//        {
		//            tag.Tag = Hammer.Common.Utilities.GenerateSlug(tag.Tag, 20);
		//            InsertBlogTag(tag);
		//        }
		//    }

		//    Save();
		//}

		//public void InsertBlogTag(BlogTag blogTag)
		//{
		//    db.BlogTags.Add(blogTag);
		//}

		//public void DeleteBlogTag(BlogTag blogTag)
		//{
		//    db.BlogTags.Remove(blogTag);
		//}

		//// Others

		//public IEnumerable<Archive> GetArchives()
		//{
		//    // Get months list
		//    DateTime dt = new DateTime(2012, 1, 1);
		//    List<SelectListItem> months = new List<SelectListItem>();

		//    for (int i = 1; i <= 12; i++)
		//    {
		//        SelectListItem item = new SelectListItem();
		//        item.Value = i.ToString();
		//        item.Text = dt.AddMonths(i-1).ToString("MMMM");
		//        months.Add(item);
		//    }

		//    // get the archives list
		//    var Archives = new List<Archive>();

		//    foreach (var item in months)  //this year per month
		//    {
		//        var Archive = new Archive();
		//        Archive.Month = item.Text;
		//        Archive.Year = DateTime.Now.Year.ToString();
		//        Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "month").Count();
		//        if (Archive.Count > 0)
		//        {
		//            Archives.Add(Archive);
		//        }
		//    }

		//    for (int i = 1; i <= 5; i++)  //last 5 years
		//    {
		//        var Archive = new Archive();
		//        Archive.Year = DateTime.Now.AddYears(-i).Year.ToString();
		//        Archive.Month = "";
		//        Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "year").Count();
		//        if (Archive.Count > 0)
		//        {
		//            Archives.Add(Archive);
		//        }
		//    }

		//    return Archives;
		//}

		//// TODO: improve this, factorize better
		//public IEnumerable<Archive> GetArchives(string languageID)
		//{
		//    // Get months list
		//    DateTime dt = new DateTime(2012, 1, 1);
		//    List<SelectListItem> months = new List<SelectListItem>();

		//    for (int i = 1; i <= 12; i++)
		//    {
		//        SelectListItem item = new SelectListItem();
		//        item.Value = i.ToString();
		//        item.Text = dt.AddMonths(i-1).ToString("MMMM");
		//        months.Add(item);
		//    }

		//    // get the archives list
		//    var Archives = new List<Archive>();

		//    foreach (var item in months)  //this year per month
		//    {
		//        var Archive = new Archive();
		//        Archive.Month = item.Text;
		//        Archive.Year = DateTime.Now.Year.ToString();
		//        Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "month", languageID).Count();
		//        if (Archive.Count > 0)
		//        {
		//            Archives.Add(Archive);
		//        }
		//    }

		//    for (int i = 1; i <= 5; i++)  //last 5 years
		//    {
		//        var Archive = new Archive();
		//        Archive.Year = DateTime.Now.AddYears(-i).Year.ToString();
		//        Archive.Month = "";
		//        Archive.Count = GetBlogsByArchive(Archive.Year, Archive.Month, "year", languageID).Count();
		//        if (Archive.Count > 0)
		//        {
		//            Archives.Add(Archive);
		//        }
		//    }

		//    return Archives;
		//}

		// Files

		public void UploadBlogPicture(Blog blog, HttpPostedFileBase file)
		{
			var oldPicture = blog.PictureFile;

			blog.PictureFile = string.Format("{0}-{1}.jpg", blog.ID, Utilities.GenerateSlug(blog.BlogTitle, 43));
			IO.UploadImageFile(file.InputStream, blog.PictureFolder, blog.PictureFile, 640);	// 640 x 280

			// if one already existed, delete
			if (!string.IsNullOrEmpty(oldPicture) && oldPicture != blog.PictureFile)
			{
				IO.DeleteFile(blog.PictureFolder, oldPicture);
			}
		}

		private void DeleteBlogPicture(Blog blog)
		{
			IO.DeleteFile(blog.PictureFolder, blog.PictureFile);
		}

		// Save

		public void Save()
		{
			db.SaveChanges();
		}
	}
}