
using ShoeShop.Models;

namespace ShoeShop.ViewModels
{
    public class BlogViewModel : Blog
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        int TopicId { get; set; }
    }
}
