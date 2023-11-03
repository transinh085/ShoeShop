using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models
{
	public class AppUser : IdentityUser
	{
		public bool? Gender { get; set; }
		public DateOnly? BirthDay { get; set; }
		public string? ProfileImageUrl { get; set; }

		public DateTime? JoinTime { get; set; } = DateTime.Now;

		public bool? Status { get; set; }

	}
}
