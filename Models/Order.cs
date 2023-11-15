using ShoeShop.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set;}
        public int PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public string Description { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool PaymentStatus { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public List<OrderDetail> Details { get; set; }
    }
}
