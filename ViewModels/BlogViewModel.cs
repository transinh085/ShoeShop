
using ShoeShop.Models;

namespace ShoeShop.ViewModels
{
    public class BlogViewModel
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public AppUser User { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Content { get; set; }
        public int TopicId { get; set; }


    }
}
