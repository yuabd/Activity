using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class Apply
	{
        /// <summary>
        /// 报名
        /// </summary>
		[Key]
		public int ApplyID { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
		public int ActiveID { get; set; }
        /// <summary>
        /// 志愿者类型ID
        /// </summary>
        public int? VolunteerID { get; set; }
        /// <summary>
        /// 报名用户
        /// </summary>
		[MaxLength(20)]
		//[Required]
		public string UserID { get; set; }
        /// <summary>
        /// 报名姓名
        /// </summary>
		public string Name { get; set; }
        /// <summary>
        /// 报名联系方式
        /// </summary>
		public string Contact { get; set; }
        /// <summary>
        /// 短号
        /// </summary>
		public string ShortCode { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
		public int? People { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
		public DateTime DateCreated { get; set; }
        /// <summary>
        /// 报名备注
        /// </summary>
		public string Content { get; set; }
        /// <summary>
        /// 是否候补报名,Y正常报名,N候补报名,V志愿者报名
        /// </summary>
		public string Backup { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public virtual Active Active { get; set; }
	}
}