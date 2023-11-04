using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels.Product;
using System.Diagnostics;
using System.Drawing;
using Image = ShoeShop.Models.Image;

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
        public async Task<IActionResult> Create([FromForm] ProductViewModel productViewModel)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Product product = new Product()
                    {
                        Name = productViewModel.Name,
                        Price = productViewModel.Price,
                        Description = productViewModel.Description,
                        IsActive = productViewModel.Status,
                        Slug = productViewModel.Slug,
                        CategoryId = Convert.ToInt32(productViewModel.Category),
                        BrandId = Convert.ToInt32(productViewModel.Brand),
                    };

                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < productViewModel.Variants.Count; i++)
                    {
                        VariantViewModel variantViewModel = productViewModel.Variants[i];
                        Variant variant = new Variant()
                        {
                            ProductId = product.Id,
                            ColorId = variantViewModel.ColorId,
                            Position = i + 1
                        };

                        _context.Add(variant);
                        await _context.SaveChangesAsync();

                        foreach (var size in variantViewModel.Sizes)
                        {
                            VariantSize variantSize = new VariantSize()
                            {
                                VariantId = variant.Id,
                                SizeId = size.SizeId,
                                Quantity = size.Stock,
                                IsActive = size.Active
                            };
                            _context.Add(variantSize);
                        }

                        foreach (var image in variantViewModel.Images)
                        {
                            if (image.Length > 0)
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                                string filePath = Path.Combine("wwwroot/img/products", uniqueFileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    image.CopyTo(fileStream);
                                }

                                Image img = new Image()
                                {
                                    Name = uniqueFileName,
                                    VariantId = variant.Id
                                };

                                _context.Add(img);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Ok(new { message = "Created product successfully!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { message = "Failed to create product." });
                }
            }
        }
    }
}
