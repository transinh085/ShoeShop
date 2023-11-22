using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoeShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		public IActionResult Profile()
		{
			return View();
		}
	}
}
