namespace ShoeShop.Models
{
    public class PaymentInformation
    {
        public List<PaymentItem> Items { get; set; }
		public decimal Amount { get; set; }
        public decimal ShippingCost { get; set; }
        public string Description { get; set; }
    }
}
