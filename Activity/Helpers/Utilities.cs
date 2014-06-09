using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.IO;

namespace Activity.Helpers
{
    public static class UExtensions
    {
        public static int Uint(this object obj)
        {
            int i = 0;
            if (obj != null && CheckIsInt(obj))
            {
                i = Convert.ToInt32(obj);
            }

            return i;
        }

        public static bool CheckIsInt(this object obj)
        {
            bool i = true;
            try
            {
                Convert.ToInt32(obj);
            }
            catch (Exception e)
            {
                i = false;
            }

            return i;
        }

        public static string UString(this object obj)
        {
            var str = "";
            if (obj != null)
            {
                str = obj.ToString();
            }
            return str;
        }
    }

	public class Utilities
	{
		public static string Substring(string source, int length)
		{
			if (source.Length > length)
			{
				return source.Substring(0, length - 3) + "...";
			}
			else
			{
				return source;
			}
		}

		public static string GenerateSlug(string phrase, int maxLength)
		{
			string str = phrase.ToLower();

			// invalid chars, make into spaces
			str = Regex.Replace(str, @"[^a-z0-9\s-\u4E00-\u9FA5]", "");		//for English language
			// convert multiple spaces/hyphens into one space  
			str = Regex.Replace(str, @"[\s-]+", " ").Trim();
			// cut and trim it
			str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
			// hyphens
			str = Regex.Replace(str, @"\s", "-");

			return str;
		}

		public static string GetFirstParagraph(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				return "";
			}

			Match m = Regex.Match(file, @"<p>\s*(.+?)\s*</p>");

			if (m.Success)
			{
				return m.Groups[1].Value;
			}

			else return "";
		}

		public static string RenderViewToString(ControllerContext context, ViewDataDictionary viewData, TempDataDictionary tempData, string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = context.RouteData.GetRequiredString("action");

			viewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
				ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		public static string RenderViewToString(ControllerContext context, string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = context.RouteData.GetRequiredString("action");

			ViewDataDictionary viewData = new ViewDataDictionary(model);

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
				ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}
