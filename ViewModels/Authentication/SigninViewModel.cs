using System.ComponentModel.DataAnnotations;

namespace ShoeShop.ViewModels.Authentication
{
    public class SigninViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}
