namespace ShoeShop.Models
{
	public class Size
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public bool? IsDelete { get; set; } = false;
    }
}
