using Microsoft.AspNetCore.Identity;

namespace ShoeShop.Models
{
	public class AppUser : IdentityUser
	{
		public bool Gender { get; set; }
		public DateTime BirthDay { get; set; }
		public string? ProfileImageUrl { get; set; }
	}
}
