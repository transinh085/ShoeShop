using System.ComponentModel.DataAnnotations;

namespace ShoeShop.ViewModels.Authentication
{
	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one non-alphanumeric character.")]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "Passwords don't match!")]
		public string ConfirmPassword { get; set; }
		public string Code { get; set; }
	}
}
