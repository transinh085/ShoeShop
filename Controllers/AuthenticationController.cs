using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;
using System.Diagnostics;

namespace ShoeShop.Controllers
{
    public class AuthenticationController : Controller
    {
		private readonly ILogger<HomeController> _logger;

		public AuthenticationController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Signin()
        {
            return View();
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
