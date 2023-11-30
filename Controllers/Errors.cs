using Microsoft.AspNetCore.Mvc;

namespace ShoeShop.Controllers
{
	public class Errors : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
