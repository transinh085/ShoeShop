using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;

namespace ShoeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
			if (_context.Products != null)
			{
				var products = await _context.Products.Include(product => product.Thumbnail)
					.OrderByDescending(product => product.CreatedAt)
					.ToListAsync();
				return View(products);
			}
			return Problem("Entity set 'AppDbContext.Products'  is null.");
		}

		public async Task<IActionResult> Detail(int? id)
		{
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Product = product;
            return View();
		}


	}
}
