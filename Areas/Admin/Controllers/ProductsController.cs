using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels.Product;
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

        public async Task<IActionResult> Index()
        {
            if (_context.Products != null)
            {
                var products = await _context.Products.Include(product => product.Category)
                    .Include(product => product.Brand)
                    .Include(product => product.Thumbnail)
                    .OrderByDescending(product => product.IsDetele)
                    .OrderByDescending(product => product.CreatedAt)
                    .ToListAsync();
                return View(products);
            }
            return Problem("Entity set 'AppDbContext.Products'  is null.");
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
                        Status = productViewModel.Status,
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

                        int thumbI = 0;
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
                                if (thumbI == variantViewModel.Thumbnail) variant.Thumbnail = img;
                                if (i == 0 && thumbI == variantViewModel.Thumbnail) product.Thumbnail = img;
                            }
                            thumbI++;
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Ok(new { message = "Created product successfully!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { message = "Failed to create product." + ex.Message });
                }
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Product = product;
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return View();
        }

        public async Task<IActionResult> GetVariant(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var variants = await _context.Variants
                .Where(v => v.ProductId == id)
                .Select(variant => new
                {
                    ColorId = variant.ColorId,
                    ColorName = variant.Color.Name,
                    Thumbnail = variant.Thumbnail.Name,
                    Images = variant.Images.Select(image => image.Name).ToList(),
                    Sizes = variant.VariantSizes.Select(size => new
                    {
                        SizeId = size.SizeId,
                        Stock = size.Quantity,
                        Active = size.IsActive,
                        SizeName = size.Size.Name
                    }).ToList()
                })
                .ToListAsync();
            return Ok(variants);
        }

        public async Task<IActionResult> Test()
        {
            var variants = await _context.Variants
                .Select(v => v.Color.Name)
                .ToListAsync();
            return Ok(variants);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDetele = true;
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            return Ok(new {message = "Delete successfully"});
        }

    }
}
