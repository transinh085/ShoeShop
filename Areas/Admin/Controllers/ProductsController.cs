using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
	{
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create()
		{
			var brands = await _context.Brands.ToListAsync();
			var categories = await _context.Categories.ToListAsync();
            var colors = await _context.Colors.ToListAsync();
            var sizes = await _context.Sizes.ToListAsync();
            ViewBag.Brands = brands;
            ViewBag.Categories = categories;
            ViewBag.Colors = colors;
            ViewBag.Sizes = sizes;
            return View();
		}
	}
}
