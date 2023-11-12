
using ShoeShop.Models;

namespace ShoeShop.ViewModels
{
    public class BlogViewModel : Blog
    {
        IFormFile Image { get; set; }
        int Thumbnail {  get; set; }
        int TopicId { get; set; }
    }
}
