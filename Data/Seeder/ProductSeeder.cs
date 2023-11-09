using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Models;
using ShoeShop.ViewModels.Product;

namespace ShoeShop.Data.Seeder
{
	public class ProductSeeder
	{
		public ProductSeeder(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				//Product 1
				ProductSeedViewModel product1 = new ProductSeedViewModel
				{
					Name = "Giày Thể Thao Nam Biti's Hunter X Festive",
					Slug = "dsmh03500",
					Description = "Giày thể thao nam Biti's Hunter X Festive DSMH03500 là mẫu giày được xếp vào " +
										"danh sách những chiếc giày thể thao nam bán chạy và tốt nhất. Nhờ sự kết hợp hoàn hảo giữa " +
										"sự thoải mái và nhẹ nhàng, bạn sẽ dễ dàng hơn trong các bài tập thể " +
										"dục hàng ngày hoặc những môn thể thao với nhịp độ cao.",
					Price = 50,
					Category = 1,
					Brand = 1,
					Status = true,
					Variants = new List<VariantViewModel>
					{
						new VariantViewModel
						{
							ColorId = 1,
							Images = new List<string>
							{
								"dsmh03500-t1.jpg",
								"dsmh03500-t2.jpg",
								"dsmh03500-t3.jpg",
								"dsmh03500-t4.webp",
								"dsmh03500-t5.jpg"
							},
							Thumbnail = 0,
							Sizes = new List<SizeViewModel>
							{
								new SizeViewModel
								{
									SizeId = 1,
									Active = true,
									Stock = 10
								},
								new SizeViewModel
								{
									SizeId = 2,
									Active = true,
									Stock = 10
								},
								new SizeViewModel
								{
									SizeId = 3,
									Active = true,
									Stock = 10
								},
							},

						},
						new VariantViewModel
						{
							ColorId = 1,
							Images = new List<string>
							{
								"dsmh03500-d1.webp",
								"dsmh03500-d2.webp",
								"dsmh03500-d3.webp",
								"dsmh03500-d4.jpg",
								"dsmh03500-d5.webp"
							},
							Thumbnail = 0,
							Sizes = new List<SizeViewModel>
							{
								new SizeViewModel
								{
									SizeId = 1,
									Active = true,
									Stock = 10
								},
								new SizeViewModel
								{
									SizeId = 2,
									Active = true,
									Stock = 10
								},
								new SizeViewModel
								{
									SizeId = 3,
									Active = true,
									Stock = 10
								},
							}
						}
					}
				};
				Create(context,product1);

				// Product 2
				// ....
			}
		}

		public void Create(AppDbContext _context, ProductSeedViewModel productViewModel)
		{
			Product product = new Product()
			{
				Name = productViewModel.Name,
				Price = productViewModel.Price,
				Description = productViewModel.Description,
				Status = productViewModel.Status,
				Slug = productViewModel.Slug,
				CategoryId = productViewModel.Category,
				BrandId = productViewModel.Category,
			};

			_context.Add(product);
			_context.SaveChanges();

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
				_context.SaveChanges();

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
					Image img = new Image()
					{
						Name = image,
						VariantId = variant.Id
					};

					_context.Add(img);
					if (thumbI == variantViewModel.Thumbnail) variant.Thumbnail = img;
					if (i == 0 && thumbI == variantViewModel.Thumbnail) product.Thumbnail = img;
					thumbI++;
				}
			}

			_context.SaveChanges();
		}
	}

	public class ProductSeedViewModel
	{
		public string? Name { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
		public bool Status { get; set; }
		public string? Slug { get; set; }
		public int Category { get; set; }
		public int Brand { get; set; }
		public List<VariantViewModel> Variants { get; set; }

	}

	public class VariantViewModel
	{
		public int ColorId { get; set; }
		public List<SizeViewModel> Sizes { get; set; }
		public List<string> Images { get; set; }
		public int Thumbnail { get; set; }
	}

	public class SizeViewModel
	{
		public int SizeId { get; set; }
		public int Stock { get; set; }
		public bool Active { get; set; }
	}
}
