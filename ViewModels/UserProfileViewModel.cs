using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShoeShop.ViewModels
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a full name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter an email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select a gender")]
        public int Gender { get; set; }

        public IFormFile? ImageFile { get; set; }
    }

}
