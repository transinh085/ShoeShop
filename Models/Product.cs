namespace ShoeShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public string Thumbnail { get; set; }

    }
}
