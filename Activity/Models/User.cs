using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
	public class User
	{
		[MaxLength(15)]
		public string UserID { get; set; }
		[MaxLength(32)]
		[Required]
		public string Password { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateLastLogin { get; set; }
		public bool IsActive { get; set; }

		public string Name { get; set; }
		public string Email { get; set; }
		public string Gender { get; set; }
		public string City { get; set; }
		public DateTime? BirthDay { get; set; }
		public string Summary { get; set; }
		public string PictureFile { get; set; }
		public string Contact { get; set; }

		public virtual ICollection<UserRoleJoin> UserRoleJoins { get; set; }
	}
}