using System.ComponentModel.DataAnnotations;

namespace ShoeShop.ViewModels.Authentication
{
	public class RegisterViewModel
	{

		[Display(Name = "Full name")]
		[Required(ErrorMessage = "Full name address is required")]
		public string FullName { get; set; }

		[Display(Name = "User name")]
		[Required(ErrorMessage = "User name address is required")]
		public string UserName { get; set; }

		[Display(Name = "Email address")]
		[Required(ErrorMessage = "Email address is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string EmailAddress { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one digit")]
		public string Password { get; set; }
		 
		[Display(Name = "Confirm password")]
		[Required(ErrorMessage = "Confirm password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password do not match")]
		public string ConfirmPassword { get; set; }
	}
}
