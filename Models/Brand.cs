namespace ShoeShop.Models
{
	public class Brand
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public bool? IsDelete { get; set; } = false;	
        public ICollection<Product>? Products { get; set; }

    }
}
