using System.Web.Mvc;

namespace Activity.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default",
				"Admin/{controller}/{action}/{id}",
				new { controller = "Active", action = "Index", id = UrlParameter.Optional },
				new[] { "Activity.Areas.Admin.Controllers" }
			);
		}
	}
}
