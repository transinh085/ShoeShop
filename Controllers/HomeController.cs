using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using System.Diagnostics;



namespace ShoeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public HomeController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
			ViewBag.Featured = await _context.Products.Where(p => p.IsFeatured == true)
				.Where(p => !p.IsDetele)
				.Include(p => p.Thumbnail)
				.OrderByDescending(p => p.CreatedAt)
				.Take(8)
				.ToListAsync();
			ViewBag.NewProduct = await _context.Products.Include(p => p.Thumbnail)
				.Where(p => !p.IsDetele)
				.OrderByDescending(p => p.CreatedAt)
				.Take(8)
				.ToListAsync();

            ViewBag.SuggestPost = await _context.Blogs.Include(p => p.Thumbnail)
                .Where (p => !p.IsDetele && p.IsPublic)
				.Include(b => b.Topic)
				.Include(b => b.User)
				.OrderByDescending(blog => blog.CreatedAt)
				.Take(4).ToListAsync();
			return View();
        }

		

        [Route("trackorder")]
        public IActionResult TrackOrder()
        {
            return View();
        }

        [Route("help")]
        public IActionResult Help()
        {
			return View();
		}

        [Route("cart")]
        public IActionResult Cart()
        {
            return View();
        }

		[Route("checkout")]
		public async Task<IActionResult> Checkout()
		{
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.currentUser = currentUser;
            ViewBag.ShippingMethod = await _context.ShippingMethods.Where(p => p.IsDelete == false).ToListAsync();
            if(currentUser != null )
            {
                ViewBag.Addresses = _context.Addresses.Where(a => a.AppUserId == currentUser.Id && !a.IsDelete).ToList();
            }
            return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

