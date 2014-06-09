using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Activity.Models;
using Activity.Service;

namespace Activity.Helpers
{
	public class ListHelper
	{
		public static IEnumerable<SelectListItem> GetRoleList()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var list = from l in db.UserRoles.AsEnumerable()
						   orderby l.RoleID
						   select new SelectListItem { Value = l.RoleID.ToString(), Text = l.RoleID };

				return list.ToList();
			}
		}

		public static IEnumerable<SelectListItem> GetBlogCategoryList()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var list = from l in db.BlogCategories.AsEnumerable()
						   orderby l.CategoryName
						   select new SelectListItem { Value = l.CategoryID.ToString(), Text = l.CategoryName };

				return list.ToList();
			}
		}

		public static IEnumerable<SelectListItem> GetCateGoriesList()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var list = from l in db.Categories.AsEnumerable()
						   orderby l.SortOrder
						   select new SelectListItem { Value = l.CategoryID.ToString(), Text = l.CategoryName };

				return list.ToList();
			}
		}

		public static IEnumerable<Blog> GetBlogs(int id)
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var list = from l in db.Blogs.AsEnumerable()
				           where l.CategoryID == id
						   orderby l.DateCreated descending 
				           select l;

				return list.ToList();
			}
		}

        public static List<Vote> GetVoteList()
        {
            using (VoteService voteService = new VoteService())
            {
                var list = voteService.GetVoteList();
                return list.ToList();
            }
        }

		public static List<string> GetTags1()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var tags = from p in db.TagJoins
						   group p by new { p.Tag } into t
						   orderby t.Count() descending
						   select t.Key.Tag;

				return tags.Take(8).ToList();
			}
		}

		public static List<string> GetTags()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var tags = (from p in db.Tags
				           where p.IsPublic == "Y"
				           select p.Tag).ToList();

                if (tags.Count() < 8)
                {
                    var count = 8 - tags.Count();
                    var tags1 = GetTags1().Where(m => !tags.Contains(m)).Take(count);

                    tags.AddRange(tags1);
                }

                return tags.ToList();
			}
		}
	}
}