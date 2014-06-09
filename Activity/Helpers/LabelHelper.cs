using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Activity.Models;

namespace Activity.Helpers
{
	public class LabelHelper
	{
		public static string UserID
		{
			get
			{
				string userID = HttpContext.Current.User.Identity.Name;

				return userID;
			}
		}

		/// <summary>
		/// 进行中
		/// </summary>
		/// <returns></returns>
		public static int GetActiveCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Activities
							where l.StartDate <= DateTime.Now && l.EndDate >= DateTime.Now && l.IsPublic == true
							&& l.UserID == UserID
							select l).ToList();

				return count.Count();
			}
		}

		public static int GetNotStartActiveCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Activities
							 where l.StartDate > DateTime.Now && l.IsPublic == true
							 && l.UserID == UserID
							 select l).ToList();

				return count.Count();
			}
		}

		/// <summary>
		/// 结束的活动
		/// </summary>
		/// <returns></returns>
		public static int GetEndActiveCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Activities
							 where l.EndDate < DateTime.Now && l.IsPublic == true
							 && l.UserID == UserID
							 select l).ToList();

				return count.Count();
			}
		}

		/// <summary>
		/// 审核中的活动
		/// </summary>
		/// <returns></returns>
		public static int GetDisActiveCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Activities
							 where l.IsPublic == false
							 && l.UserID == UserID
							 select l).ToList();

				return count.Count();
			}
		}

		public static int GetAllActiveCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Activities
							 where l.IsPublic == true
							 select l).ToList();

				return count.Count;
			}
		}

		public static int GetUserCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Users
							 select l).ToList();

				return count.Count;
			}
		}

		public static int GetJoinCount()
		{
			using (SiteDataContext db = new SiteDataContext())
			{
				var count = (from l in db.Applies
							 select l).ToList();

				return count.Count;
			}
		}

		public static string GetVisitor()
		{
			var xml = XDocument.Load(HttpContext.Current.Server.MapPath("~/XML.xml"));
			XAttribute field;

			field = (from m in xml.Descendants("visitor") select m.Attribute("value")).SingleOrDefault();
			string result = field.Value;

			return result;
		}

	}
}