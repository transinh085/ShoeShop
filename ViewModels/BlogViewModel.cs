namespace ShoeShop.ViewModels
{
    public class BlogViewModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Thumbnail { get; set; }
        public string? Topic { get; set; }
        public string? Content { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}
