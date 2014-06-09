using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class Photo
	{
        /// <summary>
        /// 图片ID
        /// </summary>
		public int PhotoID { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
		public int? ActivityID { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
		[StringLength(60)]
		public string PhotoFile { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
		[MaxLength(60)]
		public string PhotoName { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
		public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
		
		[NotMapped]
		public string PhotoFolder { get { return "/content/images"; } }

		public virtual Active Activity { get; set; }
	}
}