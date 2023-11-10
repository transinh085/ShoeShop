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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

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
				var categoriesWithCount = await _context.Categories
				   .Select(category => new
				   {
					   Category = category,
					   ProductCount = _context.Products.Count(product => product.CategoryId == category.Id)
				   })
				   .ToListAsync();
				ViewBag.Categories = categoriesWithCount;
				var brandsWithCount = await _context.Brands
				   .Select(brand => new
				   {
					   Brand = brand,
					   ProductCount = _context.Products.Count(product => product.BrandId == brand.Id)
				   })
				   .ToListAsync();
				ViewBag.Brands = brandsWithCount;
				var colorsWithCount = await _context.Colors
					.Select(color => new
					{
						Color = color,
						ProductCount = _context.Products.Count(product =>
							product.Variants.Any(variant => variant.ColorId == color.Id) // Giả định rằng có một thuộc tính ColorId trong biến variant
						)
					})
					.ToListAsync();
				ViewBag.Colors = colorsWithCount;
				var sizesWithCount = await _context.Sizes
					.Select(size => new
					{
						Size = size,
						ProductCount = _context.Products.Count(product =>
							product.Variants.Any(variant =>
								variant.VariantSizes.Any(varSize => varSize.SizeId == size.Id)
							)
						)
					})
					.ToListAsync();
				ViewBag.Sizes = sizesWithCount;
				var priceRanges = new[]
				{
					new PriceRangeInfo { Min = 0, Max = 50, Name = "$0 - $50", Value = "0:50" },
					new PriceRangeInfo { Min = 50, Max = 100, Name = "$50 - $100", Value = "50:100" },
					new PriceRangeInfo { Min = 100, Max = 200, Name = "$100 - $200", Value = "100:200" },
					new PriceRangeInfo { Min = 200, Max = 1000, Name = "$200 - $1000", Value = "200:1000" }
				};
				var productsWithPrices = new List<PriceRangeInfo>();
				foreach (var range in priceRanges)
				{
					var productCount = await _context.Products
						.CountAsync(product => product.Price >= range.Min && product.Price <= range.Max);

					productsWithPrices.Add(new PriceRangeInfo
					{
						Min = range.Min,
						Max = range.Max,
						Name = range.Name,
						Value = range.Value,
						ProductCount = productCount
					});
				}
				ViewBag.PriceRanges = productsWithPrices;
				ViewBag.Products = await _context.Products.Include(product => product.Thumbnail)
					.OrderByDescending(product => product.CreatedAt)
					.ToListAsync();
				return View();
			}
			return Problem("Entity set 'AppDbContext.Products'  is null.");
		}

		public async Task<IActionResult> Detail(int? id)
		{
            if (id == null || _context.Products == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Product = product;
			ViewBag.Related = await _context.Products
				.Include(product => product.Thumbnail)
				.Where(p => p.CategoryId == product.CategoryId)
				.OrderByDescending(product => product.CreatedAt)
				.Take(8).ToListAsync();
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


		//Api lấy ra danh sách sản phẩm
		[HttpGet, ActionName("allProducts")]
		public IActionResult GetProductList(
			int page = 1,
			int pageSize = 10,
			string query = "",
			string categories = "",
			string brands = "",
			string colors = "",
			string sizes = ""
		)
		{
			var products = _context.Products
					.Include(product => product.Thumbnail)
					.Include(product => product.Variants)
						.ThenInclude(varient => varient.VariantSizes)
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
				products = products.Where(u => bar.Contains(u.BrandId.ToString())).ToList();
			}
			if (!string.IsNullOrEmpty(colors))
			{
				string[] col = colors.Split(",");
				products = products.Where(u =>
				{
					return u.Variants.Any(item => col.Contains(item.ColorId.ToString()));
				}).ToList();
			}

			if (!string.IsNullOrEmpty(sizes))
			{
				string[] siz = sizes.Split(",");
				products = products.Where(u =>
				{
					return u.Variants.Any(variantSize =>
					{
						return variantSize.VariantSizes.Any(item => siz.Contains(item.SizeId.ToString()));
					});
				}).ToList();
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
				MaxDepth = 100,
				ReferenceHandler = ReferenceHandler.Preserve
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

public class PriceRangeInfo
{
	public int Min { get; set; }
	public int Max { get; set; }
	public string Name { get; set; }
	public string Value { get; set; }
	public int ProductCount { get; set; }
}

