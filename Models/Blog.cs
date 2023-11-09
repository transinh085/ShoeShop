namespace ShoeShop.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public string? Name { get; set; }
        public Image? Thumbnail { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int CreateBy {  get; set; }
        public int TopicID {  get; set; }
        public Topic? Topic { get; set; }
        public string? Content {get; set; }
        public bool IsDetele { get; set; } = false;
    }
}
