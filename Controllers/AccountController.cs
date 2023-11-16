using Microsoft.AspNetCore.Mvc;

namespace ShoeShop.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
