using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class BlogCategory
	{
		[Key]
		public int CategoryID { get; set; }
		[MaxLength(25)]
		[Required]
		public string CategoryName { get; set; }
		//SEO
		[MaxLength(70)]
		public string PageTitle { get; set; }
		[MaxLength(150)]
		public string MetaDescription { get; set; }
		[MaxLength(100)]
		public string MetaKeywords { get; set; }
		[MaxLength(56)]	//50 + "-" + 99,000
		public string Slug { get; set; }
		[MaxLength(2)]
		public string LanguageID { get; set; }
		public int SortOrder { get; set; }

		public virtual ICollection<Blog> Blogs { get; set; }
	}
}