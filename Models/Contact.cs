namespace ShoeShop.Models
{
	public class Contact
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
		public bool IsDeleted { get; set; } = false;
		public bool IsSeen { get; set; } = false;
    }
}
