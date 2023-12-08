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
using Microsoft.AspNetCore.Authorization;
using ShoeShop.Data.Enum;
using ShoeShop.Helpers;

namespace ShoeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;

		public ProductController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _context = context;
			_signInManager = signInManager;
			_userManager = userManager;
		}
        [Route("products")]
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
						.CountAsync(product => product.Price >= range.Min && product.Price <= range.Max ||
							product.PriceSale != 0 && product.PriceSale >= range.Min && product.PriceSale <= range.Max);

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

		[Route("/products/{slug}")]
		public async Task<IActionResult> Detail(string slug)
		{
			if (_context.Products == null) return NotFound();

			var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug);
			if (product == null || product.IsDetele) return NotFound();
			var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                ViewBag.CheckReview = 0;
            }
            else
            {
				var countReviewProduct = _context.Reviews
				   .Count(review => review.ProductId == product.Id && review.AppUserId == currentUser.Id);

				var checkByProduct = _context.Orders
					.Count(o => o.OrderStatus == OrderStatus.Confirmed &&
								o.AppUserId == currentUser.Id &&
								o.Details.Any(detail => detail.VariantSize.Variant.Product.Id == product.Id));

				ViewBag.CheckReview = (countReviewProduct < checkByProduct) ? 1 : 0;
			}

            ViewBag.Reviews = await _context.Reviews
			.Where(review => review.ProductId == product.Id)
			.OrderByDescending(review => review.CreatedAt)
			.Include(review => review.AppUser)
			.ToListAsync();

			var countReview = ViewBag.CountView = _context.Reviews.Count(review => review.ProductId == product.Id);
			ReviewStats reviewStats = new ReviewStats();
			if (countReview != 0)
			{
				var totalRating = ViewBag.TotalRating = _context.Reviews
					.Where(review => review.ProductId == product.Id)
					.Sum(review => review.Rating);
				ViewBag.AverageRating = (totalRating / (decimal)countReview).ToString("0.0");
				reviewStats.OneStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 1);
				reviewStats.PercentOneStar = ((decimal)reviewStats.OneStar / countReview * 100).ToString("0") + "%";

				reviewStats.TwoStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 2);
				reviewStats.PercentTwoStar = ((decimal)reviewStats.TwoStar / countReview * 100).ToString("0") + "%";

				reviewStats.ThreeStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 3);
				reviewStats.PercentThreeStar = ((decimal)reviewStats.ThreeStar / countReview * 100).ToString("0") + "%";

				reviewStats.FourStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 4);
				reviewStats.PercentFourStar = ((decimal)reviewStats.FourStar / countReview * 100).ToString("0") + "%";

				reviewStats.FiveStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 5);
				reviewStats.PercentFiveStar = ((decimal)reviewStats.FiveStar / countReview * 100).ToString("0") + "%";
			}
			else
			{
				ViewBag.AverageRating = 0;
				ViewBag.TotalRating = 0;
			};
			ViewBag.ReviewStats = reviewStats;
			ViewBag.Product = product;
			ViewBag.Related = await _context.Products
				.Include(product => product.Thumbnail)
				.Where(p => p.CategoryId == product.CategoryId)
				.OrderByDescending(product => product.CreatedAt)
				.Take(8).ToListAsync();
			return View();
		}


		public async Task<IActionResult> getDetailStar(int id)
		{
			if (_context.Products == null) return NotFound();

			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

			var countReview = ViewBag.CountView = _context.Reviews.Count(review => review.ProductId == product.Id);
			ReviewStats reviewStats = new ReviewStats();
			if (countReview != 0)
			{
				var totalRating = ViewBag.TotalRating = _context.Reviews
					.Where(review => review.ProductId == product.Id)
					.Sum(review => review.Rating);
				ViewBag.AverageRating = (totalRating / (decimal)countReview).ToString("0.0");
				reviewStats.OneStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 1);
				reviewStats.PercentOneStar = ((decimal)reviewStats.OneStar / countReview * 100).ToString("0") + "%";

				reviewStats.TwoStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 2);
				reviewStats.PercentTwoStar = ((decimal)reviewStats.TwoStar / countReview * 100).ToString("0") + "%";

				reviewStats.ThreeStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 3);
				reviewStats.PercentThreeStar = ((decimal)reviewStats.ThreeStar / countReview * 100).ToString("0") + "%";

				reviewStats.FourStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 4);
				reviewStats.PercentFourStar = ((decimal)reviewStats.FourStar / countReview * 100).ToString("0") + "%";

				reviewStats.FiveStar = _context.Reviews.Count(review => review.ProductId == product.Id && review.Rating == 5);
				reviewStats.PercentFiveStar = ((decimal)reviewStats.FiveStar / countReview * 100).ToString("0") + "%";
			}
			else
			{
				ViewBag.AverageRating = 0;
				ViewBag.TotalRating = 0;
			};
			ViewBag.ReviewStats = reviewStats;
			ViewBag.Product = product;
			ViewBag.Related = await _context.Products
				.Include(product => product.Thumbnail)
				.Where(p => p.CategoryId == product.CategoryId)
				.OrderByDescending(product => product.CreatedAt)
				.Take(8).ToListAsync();
			var productDetailResponse = new
			{
				TotalRating = ViewBag.TotalRating,
				CountView = ViewBag.CountView,
				AverageRating = ViewBag.AverageRating,
				ReviewStats = ViewBag.ReviewStats
			};

			return Ok(productDetailResponse);
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
						ProductSlug = v.Variant.Product.Slug,
						SizeName = v.Size.Name,
						ColorName = v.Variant.Color.Name,
						Thumbnail = v.Variant.Thumbnail.Name,
						Title = v.Variant.Product.Name,
						Price = v.Variant.Product.Price,
						PriceSale = v.Variant.Product.PriceSale,
						Stock = v.Quantity
					})
					.ToListAsync();
			return Ok(variantSize);
		}


		//Api lấy ra danh sách sản phẩm
		[HttpGet, ActionName("allProducts")]
		public IActionResult GetProductList(
			int page = 1,
			int pageSize = 9,
			string query = "",
			string categories = "",
			string brands = "",
			string colors = "",
			string sizes = "",
			string prices = "",
			string sort = "date"
		)
		{
			var queryableProducts = _context.Products
				.Where(p => !p.IsDetele)
				.Include(product => product.Thumbnail)
				.Include(product => product.Variants).ThenInclude(variant => variant.VariantSizes)
				.AsQueryable();

			// Filter by query
			if (!string.IsNullOrEmpty(query))
			{
				queryableProducts = queryableProducts.Where(u =>
					u.Slug.Contains(query, StringComparison.OrdinalIgnoreCase) ||
					u.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
					u.Description.Contains(query, StringComparison.OrdinalIgnoreCase)
				);
			}

			// Filter by categories
			if (!string.IsNullOrEmpty(categories))
			{
				string[] cate = categories.Split(',');
				queryableProducts = queryableProducts.Where(u => cate.Contains(u.CategoryId.ToString()));
			}

			// Filter by brands
			if (!string.IsNullOrEmpty(brands))
			{
				string[] bar = brands.Split(",");
				queryableProducts = queryableProducts.Where(u => bar.Contains(u.BrandId.ToString()));
			}

			// Filter by colors
			if (!string.IsNullOrEmpty(colors))
			{
				string[] col = colors.Split(",");
				queryableProducts = queryableProducts.Where(u => u.Variants.Any(item => col.Contains(item.ColorId.ToString())));
			}

			// Filter by sizes
			if (!string.IsNullOrEmpty(sizes))
			{
				string[] siz = sizes.Split(",");
				queryableProducts = queryableProducts.Where(u =>
					u.Variants.Any(variantSize =>
						variantSize.VariantSizes.Any(item => siz.Contains(item.SizeId.ToString()))
					)
				);
			}

			// Filter by prices
			if (!string.IsNullOrEmpty(prices))
			{
				var priceRangeList = PriceRangesConverter.Parse(prices);
				// Materialize the query before the price range filter
				var materializedProducts = queryableProducts.ToList();

				queryableProducts = materializedProducts.Where(product =>
					priceRangeList.Any(
						range => 
							product.Price >= range.Min && product.Price <= range.Max ||
							product.PriceSale != 0 && product.PriceSale >= range.Min && product.PriceSale <= range.Max
						)
				).AsQueryable();
			}


			// Order by
			queryableProducts = sort switch
			{
				"date" => queryableProducts.OrderByDescending(product => product.CreatedAt),
				"price" => queryableProducts.OrderBy(product => product.Price),
				"price-desc" => queryableProducts.OrderByDescending(product => product.Price),
				_ => queryableProducts.OrderByDescending(product => product.CreatedAt)
			};

			// Paginate results
			var totalItems = queryableProducts.Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var currentPageProducts = queryableProducts
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				MaxDepth = 100,
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
			};

			var result = new
			{
				CurrentPage = page,
				TotalPages = totalPages,
				TotalItems = totalItems,
				Result = currentPageProducts
			};

			return Ok(JsonSerializer.Serialize(result, options));
		}

		[HttpPost]
		public async Task<IActionResult> AddReview(ReviewViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			var review = new Review
			{
				ProductId = model.ProductId,
				Description = model.Description,
				Rating = model.Rating,
				AppUserId = user.Id
			};
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
			return Ok("Add review product successfully");
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

public class ReviewStats
{
	public int OneStar { get; set; } = 0;
	public string PercentOneStar { get; set; } = "0%";
	public int TwoStar { get; set; } = 0;
	public string PercentTwoStar { get; set; } = "0%";
	public int ThreeStar { get; set; } = 0;
	public string PercentThreeStar { get; set; } = "0%";
	public int FourStar { get; set; } = 0;
	public string PercentFourStar { get; set; } = "0%";
	public int FiveStar { get; set; } = 0;
	public string PercentFiveStar { get; set; } = "0%";
}


