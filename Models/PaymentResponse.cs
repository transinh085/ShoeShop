namespace ShoeShop.Models
{
    public class PaymentResponse
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
        public bool Success { get; set; }
    }
}
