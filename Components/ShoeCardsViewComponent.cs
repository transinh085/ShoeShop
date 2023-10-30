using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

public class ShoeCardsViewComponent : ViewComponent
{
	public IViewComponentResult Invoke(Shoe shoe)
	{
		return View(shoe);
	}
}
