using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SpecificAddress {  get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsDelete {  get; set; } = false;
    }
}
