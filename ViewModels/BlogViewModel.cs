
using ShoeShop.Models;

namespace ShoeShop.ViewModels
{
    public class BlogViewModel : Blog
    {
        public IFormFile Image { get; set; }
        public int Thumbnail {  get; set; }
        int TopicId { get; set; }
    }
}
