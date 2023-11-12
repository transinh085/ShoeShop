using ShoeShop.Models;

namespace ShoeShop.ViewModels
{
	public class PaymentViewModel
	{
		public List<CartItem> Cart { get; set; }
		public int ShippingMethodId { get; set; }
		public int PaymentMethodId { get; set; }
		public int AddressId { get; set; }
		public Address NewAddress { get; set; }
		public string OrderDescription { get; set; }
	}

	public class CartItem
	{
		public int VariantSizeId { get; set; }
		public int Quantity { get; set; }
	}
}
