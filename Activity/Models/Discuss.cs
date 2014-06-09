using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Activity.Models
{
    public class Discuss
    {
        public int ID { get; set; }
        /// <summary>
        /// 新闻或活动
        /// </summary>
        public int Type { get; set; } //新闻或活动
        /// <summary>
        /// 外链ID
        /// </summary>
        public int TargetID { get; set; }
        /// <summary>
        /// 回复父级ID
        /// </summary>
        public int? ParentID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public string LinkUrl { get; set; }
    }

	public class DiscussList
	{
		public int ID { get; set; }

		public int? ParentID { get; set; }

		public string Name { get; set; }

		public string Content { get; set; }

		public DateTime DateCreated { get; set; }

		public string UserID { get; set; }

		public string HuifuName { get; set; }

		public string LinkUrl { get; set; }
	}
}