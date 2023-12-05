using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Helpers;
using ShoeShop.Models;
using ShoeShop.ViewModels.Product;
using System.Drawing;
using System.Linq;
using Image = ShoeShop.Models.Image;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
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
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Brands = await _context.Brands.ToListAsync();
				ViewBag.Colors = await _context.Colors.ToListAsync();
				ViewBag.Sizes = await _context.Sizes.ToListAsync();

                return View();
            }
            return Problem("Entity set 'AppDbContext.Products'  is null.");
        }
        [HttpPost]
        public async Task<IActionResult> GetProducts(string query, int[] categories, int[] colors, int[] brands, string[] prices)
        {
			var draw = int.Parse(Request.Form["draw"].FirstOrDefault());
			var skip = int.Parse(Request.Form["start"].FirstOrDefault());
			var pageSize = int.Parse(Request.Form["length"].FirstOrDefault());
			var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
			var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

			var products = _context.Products
                .Include(product => product.Thumbnail)
                .Include(product => product.Category)
                .Include(product => product.Brand)
                .Where(p => !p.IsDetele)
                .AsQueryable();

			switch (sortColumn.ToLower())
			{
				case "id":
					products = sortColumnDirection.ToLower() == "asc" ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);
					break;
				case "name":
					products = sortColumnDirection.ToLower() == "asc" ? products.OrderBy(o => o.Name) : products.OrderByDescending(o => o.Name);
					break;
				case "price":
					products = sortColumnDirection.ToLower() == "asc" ? products.OrderBy(o => o.Price) : products.OrderByDescending(o => o.Price);
					break;
				default:
					products = products.OrderBy(o => o.Id);
					break;
			}

			if (!string.IsNullOrEmpty(query))
			{
				products = products.Where(m => m.Name.Contains(query));
			}

			if (categories.Length != 0)
			{
                products = products.Where(u => categories.Contains(u.CategoryId));
            }

			if (colors.Length != 0)
			{
				products = products.Where(u => u.Variants.Any(item => colors.Contains(item.ColorId)));
			}

			if (brands.Length != 0)
			{
				products = products.Where(u => brands.Contains(u.BrandId));
			}

			if (prices.Length != 0)
			{
				var priceRangeList = PriceRangesConverter.Parse(prices);
				priceRangeList.ForEach(e => Console.WriteLine(e));
				products = products.ToList().Where(product =>
					priceRangeList.Any(range =>
						(product.Price >= range.Min && product.Price <= range.Max) ||
						(product.PriceSale != 0 && product.PriceSale >= range.Min && product.PriceSale <= range.Max)
					)
				).AsQueryable();
			}

			var recordsTotal = products.Count();
			var data = products.OrderByDescending(o => o.Id).Skip(skip).Take(pageSize).ToList();
			
			var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
			return Ok(jsonData);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _context.Brands.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
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
                        PriceSale = productViewModel.PriceSale,
                        Description = productViewModel.Description,
                        Status = productViewModel.Status,
                        Label = productViewModel.Label,
                        IsFeatured = productViewModel.IsFeatured,
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


        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("/api/products/update")]
        public async Task<IActionResult> Edit([FromForm] UpdateProductViewModel productViewModel)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Product product = await _context.Products.FindAsync(productViewModel.Id);
                    if (product == null)
                    {
                        return NotFound(new { message = "Product not found." });
                    }
                    // Update product properties
                    product.Name = productViewModel.Name;
                    product.Price = productViewModel.Price;
                    product.PriceSale = productViewModel.PriceSale;
                    product.Description = productViewModel.Description;
                    product.Status = productViewModel.Status;
                    product.Label = productViewModel.Label;
                    product.IsFeatured = productViewModel.IsFeatured;
                    product.Slug = productViewModel.Slug;
                    product.CategoryId = Convert.ToInt32(productViewModel.Category);
                    product.BrandId = Convert.ToInt32(productViewModel.Brand);

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < productViewModel.Variants.Count; i++)
                    {
                        UpdateVariantViewModel variantViewModel = productViewModel.Variants[i];
                        Variant variant;
                        if (variantViewModel.VariantId > 0)
                        {
                            variant = await _context.Variants.FindAsync(variantViewModel.VariantId);
                            variant.ColorId = variantViewModel.ColorId;
                            variant.Position = i + 1;
                        }
                        else
                        {
                            variant = new Variant()
                            {
                                ProductId = product.Id,
                                ColorId = variantViewModel.ColorId,
                                Position = i + 1
                            };

                            _context.Add(variant);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var sizeViewModel in variantViewModel.Sizes)
                        {
                            VariantSize variantSize = await _context.VariantSizes
                                .FirstOrDefaultAsync(v => v.VariantId == variant.Id && v.SizeId == sizeViewModel.SizeId);

                            if (variantSize != null)
                            {
                                variantSize.SizeId = sizeViewModel.SizeId;
                                variantSize.Quantity = sizeViewModel.Stock;
                                variantSize.IsActive = sizeViewModel.Active;
								_context.Update(variantSize);
                            }
                            else
                            {
                                variantSize = new VariantSize()
                                {
                                    VariantId = variant.Id,
                                    SizeId = sizeViewModel.SizeId,
                                    Quantity = sizeViewModel.Stock,
                                    IsActive = sizeViewModel.Active
                                };
                                _context.Add(variantSize);
                            }
                        }

                        var existingImages = await _context.Images
                            .Where(img => img.VariantId == variant.Id)
                            .ToListAsync();
                        foreach (var existingImage in existingImages)
                        {
                            var filePath = Path.Combine("wwwroot/img/products", existingImage.Name);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            _context.Images.Remove(existingImage);
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

                    return Ok(new { message = "Product updated successfully!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { message = "Failed to update product." + ex.Message });
                }
            }
        }

		[AllowAnonymous]
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
                    VariantId = variant.Id,
                    ColorId = variant.ColorId,
                    ColorName = variant.Color.Name,
                    Thumbnail = variant.Thumbnail.Name,
                    Images = variant.Images.Select(image => image.Name).ToList(),
					Position = variant.Position,
					Sizes = variant.VariantSizes.Select(size => new
                    {
                        VariantSizeId = size.Id,
                        SizeId = size.SizeId,
                        Stock = size.Quantity,
                        Active = size.IsActive,
                        SizeName = size.Size.Name
                    }).ToList()
                })
                .OrderBy(v => v.Position)
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

        [HttpPost]
        public async Task<IActionResult> CheckSlug(string slug)
        {
            var product = await _context.Products.FirstOrDefaultAsync(v => v.Slug == slug);
            return Json(new { IsUnique = product == null });
        }
        [HttpPost]
        public async Task<IActionResult> CheckSlugUpdate(int id, string slug)
        {
            var product = await _context.Products.Where(p => p.Id != id)
                .FirstOrDefaultAsync(v => v.Slug == slug);
            return Json(new { IsUnique = product == null });
        }
    }
}
