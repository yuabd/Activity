using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class Active
	{
		public int ActiveID { get; set; }
        /// <summary>
        /// 发布活动人的ID
        /// </summary>
		public string UserID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
		public int CategoryID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
		[MaxLength(100)]
		public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
		[Column(TypeName = "ntext")]
		[MaxLength]
		public string Content { get; set; }
        /// <summary>
        /// 活动地址
        /// </summary>
		[MaxLength(100)]
		public string Address { get; set; }
        /// <summary>
        /// 活动费用
        /// </summary>
		[MaxLength(60)]
		public string Price { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
		public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
		public DateTime? EndDate { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
		public DateTime? DateCreated { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
		public bool IsPublic { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
		public string PictureFile { get; set; }
        /// <summary>
        /// 可报名人数
        /// </summary>
		public int People { get; set; }
        /// <summary>
        /// 活动总结内容
        /// </summary>
		[MaxLength]
		[Column(TypeName="ntext")]
		public string EndContent { get; set; }
        /// <summary>
        /// 活动标签
        /// </summary>
		public string Tags { get; set; }
        /// <summary>
        /// 宣传图
        /// </summary>
		public string HomePictureFile { get; set; }
        /// <summary>
        /// 赞
        /// </summary>
		public int? Good { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>
		public int PageVisits { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// 活动发起人
        /// </summary>
        public string ContactPeople { get; set; }
        /// <summary>
        /// 活动联系人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string PersonPhone { get; set; }
        /// <summary>
        /// 报名提醒事项
        /// </summary>
        public string Remind { get; set; }
        /// <summary>
        /// 是否开启讨论
        /// </summary>
		public string IsDiscuss { get; set; }
        /// <summary>
        /// 志愿者人数
        /// </summary>
        public int VolunteerCount { get; set; }
        /// <summary>
        /// 是否优先报名
        /// </summary>
        public bool IsVolunteerFirst { get; set; }

        /// <summary>
        /// 图片文件夹
        /// </summary>
		[NotMapped]
		public string PictureFolder { get { return "/content/images"; } }
        /// <summary>
        /// 状态
        /// </summary>
		[NotMapped]
		public string Status
		{
			get
			{
				if (StartDate > DateTime.Now)
				{
					return "NoStart";
				}
				else if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
				{
					return "Normal";
				}
				else
				{
					return "End";
				}
			}
		}
        /// <summary>
        /// 用户是否已经报名
        /// </summary>
		[NotMapped]
		public string AlreadyApply
		{
			get
			{
				var a = Applies.FirstOrDefault(m => m.UserID == HttpContext.Current.User.Identity.Name);
				if (a != null)
				{
					return "Y";
				}
				else
				{
					return "N";
				}
			}
		}

        /// <summary>
        /// 图片
        /// </summary>
		public virtual ICollection<Photo> Photos { get; set; }
		public virtual User User { get; set; }
		public virtual Category Category{ get; set; }
		public virtual ICollection<Apply> Applies { get; set; }
		public virtual ICollection<TagJoin> TagJoins { get; set; }
	}
}