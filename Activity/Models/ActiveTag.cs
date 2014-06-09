using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class ActiveTag
	{
        /// <summary>
        /// 标签
        /// </summary>
		[Key]
		public string Tag { get; set; }

		public string IsPublic { get; set; }

		public virtual ICollection<TagJoin> TagJoins { get; set; }
	}
}