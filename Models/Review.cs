using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
	public class Review
	{
		public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int Rating { get; set; }
		public string Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public bool? IsDelete { get; set; } = false;
    }
}
