namespace ShoeShop.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public string? Thumbnail { get; set; }
        public int Position { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<VariantSize>? VariantSizes { get; set; }
    }
}
