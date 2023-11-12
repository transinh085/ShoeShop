namespace ShoeShop.Models
{
	public class PaymentItem
	{
		public int ProductId { get; set; }
		public string Title { get; set; }
		public int VariantSizeId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; } = 0;
	}
}
