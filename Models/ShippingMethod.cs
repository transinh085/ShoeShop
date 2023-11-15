namespace ShoeShop.Models
{
	public class ShippingMethod
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Cost { get; set; }

		public bool IsDelete { get; set; } = false;
	}
}
