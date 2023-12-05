using System.ComponentModel.DataAnnotations;

namespace ShoeShop.ViewModels.Authentication
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
