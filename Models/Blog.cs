using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string? Name { get; set; }
        public Image? Thumbnail { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? publicDate { get; set; }
        public AppUser? User { get; set; }
        public int TopicID { get; set; }
        public Topic? Topic { get; set; }
        public string? Content { get; set; }
        public bool IsPublic { get; set; }=false;
        public bool IsDetele { get; set; } = false;

    }
}
