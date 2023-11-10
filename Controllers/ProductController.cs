using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using Bogus.DataSets;

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
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Brands = await _context.Brands.ToListAsync();
				ViewBag.Colors = await _context.Colors.ToListAsync();
				ViewBag.Products = await _context.Products.Include(product => product.Thumbnail)
					.OrderByDescending(product => product.CreatedAt)
					.ToListAsync();
				return View();
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

		[HttpPost]
		public async Task<IActionResult> GetCart([FromBody]CartViewModel cart)
		{
			var variantSize = await _context.VariantSizes
					.Where(v => cart.Carts.Contains(v.Id))
					.Select(v => new
					{
						VariantSizeId = v.Id,
						ProductId = v.Variant.ProductId,
						SizeName = v.Size.Name,
						ColorName = v.Variant.Color.Name,
						thumbnail = v.Variant.Thumbnail.Name,
						Title = v.Variant.Product.Name,
						Price = v.Variant.Product.Price
					})
					.ToListAsync();
			return Ok(variantSize);
		}

		[HttpGet, ActionName("allProducts")]
		public IActionResult GetProductList(
			int page = 1,
			int pageSize = 10,
			string query = "",
			string categories = "",
			string brands = "",
			string colors = ""
		)
		{
			var products = _context.Products
					.Include(product => product.Thumbnail)
					.OrderByDescending(product => product.CreatedAt)
					.ToList();
			if (!string.IsNullOrEmpty(query))
			{
				products = products.Where(u =>
					u.Slug.Contains(query, StringComparison.OrdinalIgnoreCase) ||
					u.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
					)
					.ToList();
			}
			if (!string.IsNullOrEmpty(categories))
			{
				string[] cate = categories.Split(',');
				products = products.Where(u => cate.Contains(u.CategoryId.ToString())).ToList();
			}
			if(!string.IsNullOrEmpty(brands))
			{
				string[] bar = brands.Split(",");
				products = products.Where(u => brands.Contains(u.BrandId.ToString())).ToList();
			}
			if (page < 1)
			{
				page = 1;
			}

			// Tính tổng số trang dựa trên số lượng sản phẩm và kích thước trang
			int totalItems = products.Count;
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			// Lấy danh sách sản phẩm của trang hiện tại
			var currentPageProduct = products
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				MaxDepth = 10
			};

			var result = new
			{
				CurrentPage = page,
				TotalPages = totalPages,
				result = currentPageProduct
			};

			string json = JsonSerializer.Serialize(result, options);

			return new ContentResult
			{
				Content = json,
				ContentType = "application/json",
			};
		}
	}
}
