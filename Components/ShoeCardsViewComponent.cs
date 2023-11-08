using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;

public class ShoeCardsViewComponent : ViewComponent
{
	public IViewComponentResult Invoke(Product shoe)
	{
		return View(shoe);
	}
}
