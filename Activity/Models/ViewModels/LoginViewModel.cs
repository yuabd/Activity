using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models.ViewModels
{
	public class LoginViewModel
	{
		//[Required(ErrorMessage = "Required")]
		public string UserID { get; set; }

		//[Required(ErrorMessage = "Required")]
		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public LoginViewModel()
		{
		}
	}
}