using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class TagJoin
	{
		[Key, Column(Order=1)]
		public int ActiveID { get; set; }
		[Key, Column(Order=2)]
		public string Tag { get; set; }

		public virtual ActiveTag ActiveTag { get; set; }
		public virtual Active Active { get; set; }
	}
}