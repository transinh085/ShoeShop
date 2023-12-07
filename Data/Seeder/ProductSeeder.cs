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
					PriceSale = 30,
                    Label = 1,
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
							Thumbnail = 2,
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

				//Product 3
				ProductSeedViewModel product3 = new ProductSeedViewModel
				{
					Name = "HumanraceSamba",
					Slug = "humanracesamba",
					Description = "Giày thể thao nam HumanraceSamba là mẫu giày được xếp vào " +
										"danh sách những chiếc giày thể thao nam bán chạy và tốt nhất. Nhờ sự kết hợp hoàn hảo giữa " +
										"sự thoải mái và nhẹ nhàng, bạn sẽ dễ dàng hơn trong các bài tập thể " +
										"dục hàng ngày hoặc những môn thể thao với nhịp độ cao.",
					Price = 60,
                    PriceSale = 50,
                    Label = 1,
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
								"HumanraceSambaWhite1.webp",
								"HumanraceSambaWhite2.webp",
								"HumanraceSambaWhite3.webp",
								"HumanraceSambaWhite4.webp",
							},
							Thumbnail = 2,
							Sizes = new List<SizeViewModel>
							{
								new SizeViewModel
								{
									SizeId = 1,
									Active = true,
									Stock = 2
								},
								new SizeViewModel
								{
									SizeId = 2,
									Active = true,
									Stock = 3
								},
								new SizeViewModel
								{
									SizeId = 3,
									Active = true,
									Stock = 5
								},
							},

						}
					}
				};

				//Product 4
				ProductSeedViewModel product4 = new ProductSeedViewModel
				{
					Name = "Questart",
					Slug = "questart",
					Description = "Giày thể thao nam Questart là mẫu giày được xếp vào " +
										"danh sách những chiếc giày thể thao nam bán chạy và tốt nhất. Nhờ sự kết hợp hoàn hảo giữa " +
										"sự thoải mái và nhẹ nhàng, bạn sẽ dễ dàng hơn trong các bài tập thể " +
										"dục hàng ngày hoặc những môn thể thao với nhịp độ cao.",
					Price = 120,
                    PriceSale = 90,
                    Label = 1,
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
								"QUESTARWhite3.webp",
								"QUESTARWhite1.webp",
								"QUESTARWhite4.webp",
								"QUESTARWhite2.webp",
							},
							Thumbnail = 2,
							Sizes = new List<SizeViewModel>
							{
								new SizeViewModel
								{
									SizeId = 1,
									Active = true,
									Stock = 2
								},
								new SizeViewModel
								{
									SizeId = 2,
									Active = true,
									Stock = 3
								},
								new SizeViewModel
								{
									SizeId = 3,
									Active = true,
									Stock = 5
								},
							},
						},
						new VariantViewModel
						{
							ColorId = 2,
							Images = new List<string>
							{
								"QUESTARBlack2.webp",
								"QUESTARBlack3.webp",
								"QUESTARBlack4.webp",
								"QUESTARBlack1.webp",
							},
							Thumbnail = 2,
							Sizes = new List<SizeViewModel>
							{
								new SizeViewModel
								{
									SizeId = 1,
									Active = true,
									Stock = 2
								},
								new SizeViewModel
								{
									SizeId = 2,
									Active = true,
									Stock = 3
								},
								new SizeViewModel
								{
									SizeId = 3,
									Active = true,
									Stock = 5
								},
							},
						}
					}
				};
				Create(context, product1);
				Create(context, product3);
				Create(context, product4);
                ProductSeedViewModel producth1 = new ProductSeedViewModel
                {
                    Name = "Giày VINTAS NAUDA EXT - LOW TOP - MONK'S ROBE",
                    Slug = "AV00203",
                    Description = "Đây là sản phẩm với bảng sze mới, tối ưu hơn khi Ananas có sự bổ sung thêm những size lẻ từ ngày 01/06/2023, số liệu có thể có sự khác đi đôi chút so với các thông số được in trong một số mẫu giày đã phát hành.",
                    Price = 650000,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    Variants = new List<VariantViewModel>
                    {
                        new VariantViewModel
                        {
                            ColorId = 8,
                            Images = new List<string>
                            {
                                "Pro_AV00203_1.jpeg",
                                "Pro_AV00203_2.jpeg",
                                "Pro_AV00203_3.jpeg",
                                "Pro_AV00203_4.jpeg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                    }
                };
                // Product hiu_prd2
                ProductSeedViewModel producth2 = new ProductSeedViewModel
                {
                    Name = "Giày PATTAS TOMO - HIGH TOP - OFFWHITE",
                    Slug = "AV00182",
                    Description = "Đây là sản phẩm với bảng sze mới, tối ưu hơn khi Ananas có sự bổ sung thêm những size lẻ từ ngày 01/06/2023, số liệu có thể có sự khác đi đôi chút so với các thông số được in trong một số mẫu giày đã phát hành.",
                    Price = 750000,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    Variants = new List<VariantViewModel>
                    {
                        new VariantViewModel
                        {
                            ColorId = 1,
                            Images = new List<string>
                            {
                                "Pro_AV00182_1.jpeg",
                                "Pro_AV00182_2.jpeg",
                                "Pro_AV00182_3.jpeg",
                                "Pro_AV00182_4.jpeg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },

                    }
                };

                // Product hiu_prd3
                ProductSeedViewModel producth3 = new ProductSeedViewModel
                {
                    Name = "Giày TRACK 6 2.BLUES - LOW TOP - NAVY BLUE",
                    Slug = "A6T014",
                    Description = "Đây là sản phẩm với bảng sze mới, tối ưu hơn khi Ananas có sự bổ sung thêm những size lẻ từ ngày 01/06/2023, số liệu có thể có sự khác đi đôi chút so với các thông số được in trong một số mẫu giày đã phát hành.",
                    Price = 1190000,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    Variants = new List<VariantViewModel>
                    {
                        new VariantViewModel
                        {
                            ColorId = 4,
                            Images = new List<string>
                            {
                                "Pro_A6T014_1.jpeg",
                                "Pro_A6T014_2.jpeg",
                                "Pro_A6T014_3.jpeg",
                                "Pro_A6T014_4.jpeg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                         new VariantViewModel
                        {
                            ColorId = 4,
                            Images = new List<string>
                            {
                                "Pro_A6T015_1.jpeg",
                                "Pro_A6T015_2.jpeg",
                                "Pro_A6T015_3.jpeg",
                                "Pro_A6T015_4.jpeg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                    }
                };


                // Product hiu_prd4
                ProductSeedViewModel producth4 = new ProductSeedViewModel
                {
                    Name = "Giày URBAS SC - HIGH TOP - FOLIAGE",
                    Slug = "AV00194",
                    Description = "Đây là sản phẩm với bảng sze mới, tối ưu hơn khi Ananas có sự bổ sung thêm những size lẻ từ ngày 01/06/2023, số liệu có thể có sự khác đi đôi chút so với các thông số được in trong một số mẫu giày đã phát hành.",
                    Price = 650000,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    Variants = new List<VariantViewModel>
                    {
                        new VariantViewModel
                        {
                            ColorId = 5,
                            Images = new List<string>
                            {
                                "Pro_AV00194_1.jpg",
                                "Pro_AV00194_2.jpg",
                                "Pro_AV00194_3.jpg",
                                "Pro_AV00194_4.jpg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                        new VariantViewModel
                        {
                            ColorId = 9,
                            Images = new List<string>
                            {
                                "Pro_AV00192_1.jpg",
                                "Pro_AV00192_2.jpg",
                                "Pro_AV00192_3.jpg",
                                "Pro_AV00192_4.jpg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
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
                                "Pro_AV00174_1.jpeg",
                                "Pro_AV00174_2.jpeg",
                                "Pro_AV00174_3.jpeg",
                                "Pro_AV00174_4.jpeg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                    }
                };

                // Product hiu_prd5
                ProductSeedViewModel producth5 = new ProductSeedViewModel
                {
                    Name = "Giày URBAS SC - MULE - DARK PURPLE",
                    Slug = " AV00197",
                    Description = "Đây là sản phẩm với bảng sze mới, tối ưu hơn khi Ananas có sự bổ sung thêm những size lẻ từ ngày 01/06/2023, số liệu có thể có sự khác đi đôi chút so với các thông số được in trong một số mẫu giày đã phát hành.",
                    Price = 580000,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    Variants = new List<VariantViewModel>
                    {
                        new VariantViewModel
                        {
                            ColorId = 10,
                            Images = new List<string>
                            {
                                "Pro_AV00197_1.jpg",
                                "Pro_AV00197_2.jpg",
                                "Pro_AV00197_3.jpg",
                                "Pro_AV00197_4.jpg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                        new VariantViewModel
                        {
                            ColorId = 9,
                            Images = new List<string>
                            {
                                "Pro_AV00200_1.jpg",
                                "Pro_AV00200_2.jpg",
                                "Pro_AV00200_3.jpg",
                                "Pro_AV00200_4.jpg",
                            },
                            Thumbnail = 0,
                            Sizes = new List<SizeViewModel>
                            {
                                new SizeViewModel
                                {
                                    SizeId = 2,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 3,
                                    Active = false,
                                    Stock = 10
                                },
                                new SizeViewModel
                                {
                                    SizeId = 4,
                                    Active = true,
                                    Stock = 10
                                },
                            },

                        },
                    }
                };
                Create(context, producth1);
                Create(context, producth2);
                Create(context, producth3);
                Create(context, producth4);
                Create(context, producth5);
            }
        }
        

        public void Create(AppDbContext _context, ProductSeedViewModel productViewModel)
		{
			Product product = new Product()
			{
				Name = productViewModel.Name,
				Price = productViewModel.Price,
				PriceSale = productViewModel.PriceSale,
				Label = productViewModel.Label,
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
		public decimal PriceSale { get; set; }
		public string? Description { get; set; }
		public bool Status { get; set; } = true;
		public string? Slug { get; set; }
		public int Category { get; set; }
		public int Label { get; set; } = 0;
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
