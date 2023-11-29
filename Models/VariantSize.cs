namespace ShoeShop.Models
{
    public class VariantSize
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public Variant Variant { get; set; }
        public int SizeId { get; set; }
        public Size? Size { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
