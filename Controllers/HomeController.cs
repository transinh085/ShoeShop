using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;
using System.Diagnostics;

namespace ShoeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("treackorder")]
        public IActionResult TrackOrder()
        {
            return View();
        }

        [Route("help")]
        public IActionResult Help()
        {
			return View();
		}

        [Route("products")]
        public IActionResult Products()
        {
            return View();
        }

        [Route("cart")]
        public IActionResult Cart()
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