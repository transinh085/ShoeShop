namespace ShoeShop.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string? Slug { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } 
        public Image? Thumbnail { get; set; }
        public bool IsDetele {  get; set; }
        public bool Status { get; set; }  
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Variant> Variants { get; set; }

        public Product()
        {
            IsDetele = false;
        }
    }
}
