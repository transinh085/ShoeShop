using System.ComponentModel.DataAnnotations;

namespace ShoeShop.ViewModels
{
	public class ReviewViewModel
	{
		[Required(ErrorMessage = "ProductId is required")]
		public int ProductId { get; set; }
		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Rating is required")]
		public int Rating { get; set; }
	}
}
