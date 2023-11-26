using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public int? Gender { get; set; } = 1;
        public DateTime? BirthDay { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Image { get; set; }

        public DateTime? JoinTime { get; set; } = DateTime.Now;

        public bool? Status { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public List<Address>? Addresses { get; set; }
    }
}
