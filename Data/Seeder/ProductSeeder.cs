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
                    IsFeatured = true,
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
                    IsFeatured = true,
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
                    Price = 26,
                    Category = 2,
                    Brand = 4,
                    Status = true,
                    IsFeatured = true,
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
                    Price = 24,
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
				//Product_An_1
				ProductSeedViewModel product_an1 = new ProductSeedViewModel
				{
					Name = "Giày Thể Thao Nam Biti’s Hunter X Liteflex 2.0 - Midnight 2K23 HSM004401",
					Slug = "hsm004401",
					Description = "Giày Thể Thao Nam Biti’s Hunter X Lite Flex 2.0 - Midnight 2K23 HSM004401 trong bộ sưu tập BITI'S HUNTER STREET MIDNIGHT 2K23" +
								" được rất nhiều người tiêu dùng Việt mê mẩn ngay từ lần đầu tiên. Bởi, không chỉ có màu sắc đẹp mắt, sản phẩm còn có chất lượng cao cấp và giá thành phải chăng. ",
					Price = 150,
					PriceSale = 120,
					Label = 1,
					Category = 2,
					Brand = 3,
					Status = true,
					Variants = new List<VariantViewModel>
				{
					new VariantViewModel
					{
						ColorId = 2,
						Images = new List<string>
						{
							"hsm004401den1.png",
							"hsm004401den2.jpg",
							"hsm004401den3.webp",
							"hsm004401den4.jpg",
							"hsm004401den5.jpg",
						},
						Thumbnail = 1,
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

				}
				};


				//Product_An_2
				ProductSeedViewModel product_an2 = new ProductSeedViewModel
				{
					Name = "Giày Thể Thao Nam Biti's Hunter Street HSM004700",
					Slug = "hsm004700",
					Description = "Giày Thể Thao Nam Biti's Hunter Street HSM004700 là một trong những sản phẩm dành riêng cho phái mạnh được đông đảo khách hàng Việt Nam yêu thích. " +
								"Không chỉ bởi thiết kế đẹp mắt và thời trang, đôi giày để lại ấn tượng khác biệt vì chất lượng, màu sắc và giá thành.",
					Price = 130,
					PriceSale = 99,
					Label = 1,
					Category = 1,
					Brand = 3,
					Status = true,
                    IsFeatured = true,
                    Variants = new List<VariantViewModel>
				{
					new VariantViewModel
					{
						ColorId = 8,
						Images = new List<string>
						{
							"hsm004700nau1.webp",
							"hsm004700nau2.webp",
							"hsm004700nau3.webp",
							"hsm004700nau4.webp",
							"hsm004700nau5.jpg",
							"hsm004700nau6.webp",
						},
						Thumbnail = 1,
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
						ColorId = 4,
						Images = new List<string>
						{
							"hsm004700xnh1.webp",
							"hsm004700xnh2.webp",
							"hsm004700xnh3.webp",
							"hsm004700xnh4.webp",
							"hsm004700xnh5.jpg",
							"hsm004700xnh6.webp",
						},
						Thumbnail = 1,
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

				}
				};

				//Product_An_3
				ProductSeedViewModel product_an3 = new ProductSeedViewModel
				{
					Name = "Giày Thể Thao Cao Cấp Nam Biti's Hunter Layered Upper DSMH02800",
					Slug = "dsmh02800",
					Description = "Để luyện tập thể thao tốt thì chúng ta cần phải tìm được đôi giày đơn giản và phù hợp với mình. Giày thể thao cao cấp nam Biti's Hunter Layered Upper DSMH02800 là một trong mẫu giày nổi tiếng về cá tính và được sản xuất trên công nghệ hiện đại bậc nhất. " +
					"Giày được thiết kế giúp hạn chế tối đa thương thích và đảm bảo sự cân bằng, độ bền bỉ khi di chuyển.",
					Price = 150,
					PriceSale = 120,
					Label = 1,
					Category = 2,
					Brand = 3,
					Status = true,
					Variants = new List<VariantViewModel>
				{
					new VariantViewModel
					{
						ColorId = 2,
						Images = new List<string>
						{
							"dsmh02800den1.webp",
							"dsmh02800den2.webp",
							"dsmh02800den3.webp",
							"dsmh02800den4.webp",
							"dsmh02800den5.webp",
							"dsmh02800den6.webp",
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
							"dsmh02800trang1.webp",
							"dsmh02800trang2.webp",
							"dsmh02800trang3.webp",
							"dsmh02800trang4.webp",
							"dsmh02800trang5.webp",
							"dsmh02800trang6.webp",
						},
						Thumbnail = 1,
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

				}
				};
				//Product_An_4
				ProductSeedViewModel product_an4 = new ProductSeedViewModel
				{
					Name = "Giày Thể Thao Nam Biti's Hunter Street Z Collection DSMH06200",
					Slug = "dsmh06200",
					Description = "Một đôi giày thể thao chất lượng là một “người bạn đồng hành” không thể thiếu của những dân chơi thể thao chuyên nghiệp. " +
					"Theo đó, nếu chưa biết lựa chọn mẫu giày nào phù hợp, bạn hãy cân nhắc sản phẩm “quốc dân” " +
					"vừa được Biti’s trình làng: Giày Thể Thao Nam Biti's Hunter Street Z Collection DSMH06200. Cùng tìm hiểu chi tiết nhé!",
					Price = 100,
					PriceSale = 80,
					Label = 1,
					Category = 1,
					Brand = 3,
					Status = true,
					Variants = new List<VariantViewModel>
				{
					new VariantViewModel
					{
						ColorId = 1,
						Images = new List<string>
						{
							"dsmh02600_1.webp",
							"dsmh02600_2.webp",
							"dsmh02600_3.webp",
							"dsmh02600_4.webp",
							"dsmh02600_5.webp",
							"dsmh02600_6.webp",
						},
						Thumbnail = 1,
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
				}
				};
				//Product_An_5
				ProductSeedViewModel product_an5 = new ProductSeedViewModel
				{
					Name = "GIÀY PUREBOOST 23",
					Slug = "pureboost23",
					Description = "Vui vẻ chinh phục cự ly mỗi ngày với đôi giày chạy bộ adidas Pureboost 23. Đế BOOST toàn phần cho khả năng hoàn trả năng lượng tối ưu, " +
					"cùng các đường khoét bố trí hợp lý dọc đế ngoài bằng cao su cho phép mút foam nén chặt và giãn nở khi chạy bộ. " +
					"Thân giày bằng vải lưới kỹ thuật siêu nhẹ, thoáng khí ở các vùng bàn chân tăng nhiệt và nâng đỡ ở những vị trí cần thiết, " +
					"mang lại trải nghiệm chạy bộ thoải mái cho cả bàn chân.",
					Price = 170,
					PriceSale = 150,
					Label = 1,
					Category = 3,
					Brand = 1,
					Status = true,
                    IsFeatured = true,
                    Variants = new List<VariantViewModel>
				{
					new VariantViewModel
					{
						ColorId = 5,
						Images = new List<string>
						{
							"pureboost_xanh_1.webp",
							"pureboost_xanh_2.webp",
							"pureboost_xanh_3.webp",
							"pureboost_xanh_4.jpg",
							"pureboost_xanh_5.webp",
							"pureboost_xanh_6.webp",
						},
						Thumbnail = 0,
						Sizes = new List<SizeViewModel>
						{
							new SizeViewModel
							{
								SizeId = 3,
								Active = true,
								Stock = 10
							},
							new SizeViewModel
							{
								SizeId = 4,
								Active = true,
								Stock = 10
							},
							new SizeViewModel
							{
								SizeId = 5,
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
							"pureboost_t_1.webp",
							"pureboost_t_2.webp",
							"pureboost_t_3.webp",
							"pureboost_t_4.webp",
							"pureboost_t_5.webp",

						},
						Thumbnail = 1,
						Sizes = new List<SizeViewModel>
						{
							new SizeViewModel
							{
								SizeId = 3,
								Active = true,
								Stock = 10
							},
							new SizeViewModel
							{
								SizeId = 4,
								Active = true,
								Stock = 10
							},
							new SizeViewModel
							{
								SizeId = 5,
								Active = true,
								Stock = 10
							},
						},

					},

				}
				};
				Create(context, product_an1);
				Create(context, product_an2);
				Create(context, product_an3);
				Create(context, product_an4);
				Create(context, product_an5);

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
				IsFeatured = productViewModel.IsFeatured,
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
        public bool IsFeatured { get; set; } = false;
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
