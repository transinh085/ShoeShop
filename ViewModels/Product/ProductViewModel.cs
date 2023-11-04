using Newtonsoft.Json;

namespace ShoeShop.ViewModels.Product
{
    public class ProductViewModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string? Slug { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public List<VariantViewModel>? Variants { get; set; }
    }

    public class VariantViewModel
    {
        public string? ColorId { get; set; }
        public List<SizeViewModel>? Sizes { get; set; }
        public List<IFormFile> Images { get; set; }
        public int Thumbnail { get; set; }
    }

    public class SizeViewModel
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public bool Active { get; set; }
    }
}
