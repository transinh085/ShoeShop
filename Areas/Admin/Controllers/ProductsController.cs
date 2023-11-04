using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.ViewModels.Product;
using System.Diagnostics;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
	{
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] ProductViewModel productViewModel)
        {
            string? productName = productViewModel.Name;
            Console.WriteLine(productName);
            decimal productPrice = productViewModel.Price;
            string productDescription = productViewModel.Description;
            bool productStatus = productViewModel.Status;
            string productSlug = productViewModel.Slug;
            string productCategory = productViewModel.Category;
            string productBrand = productViewModel.Brand;

            foreach (var variant in productViewModel.Variants)
            {
                string colorId = variant.ColorId;

                foreach (var size in variant.Sizes)
                {
                    int sizeId = size.Id;
                    int sizeStock = size.Stock;
                    bool sizeActive = size.Active;
                }

                foreach (var image in variant.Images)
                {
                    if (image.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine("wwwroot/img/products", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                        Console.WriteLine(filePath);
                    }
                }
            }

            // Xử lý dữ liệu sản phẩm và hình ảnh theo nhu cầu của bạn

            return Ok(new { message = "Dữ liệu sản phẩm đã được xử lý thành công!" });
        }
    }
}
