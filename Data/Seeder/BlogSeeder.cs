using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShoeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoeShop.Data.Seeder
{
	public class BlogSeeder
	{
		private readonly UserManager<AppUser> _userManager;

		public BlogSeeder(IApplicationBuilder applicationBuilder, UserManager<AppUser> userManager)
		{
			_userManager = userManager;

			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
				context.Database.EnsureCreated();

				if (!context.Blogs.Any())
				{
					// Retrieve a user for assignment to the Blog
					var user = _userManager.FindByEmailAsync("andinh1443@gmail.com").Result;

					context.Blogs.AddRange(new List<Blog>()
					{
						new Blog
						{
							Name = "TOP giày mùa đông đẹp mắt, chất lượng nên sở hữu trong năm",
							Slug = "top-giay-mua-dong-dep",
							Thumbnail= new Image()
							{
								Name="blogThumb1.webp",
								VariantId=null
							},
							Description = "Mùa đông sắp tới, bạn cần tìm cho mình một đôi giày mang êm chắc chân, độ bền cao? Thế nhưng, bạn chưa biết nên mua kiểu nào, mua ở đâu mới chất lượng?" +
								" Hãy tham khảo ngay những mẫu giày mùa đông chính hãng Biti’s được giới thiệu trong bài viết sau, cùng kinh nghiệm chọn mua hữu ích ngay nhé!",
							Content = "Những kiểu giày mùa đông cho nam đáng mua nhất năm nay\r\n" +
								"Sau đây, Biti’s tổng hợp cho bạn TOP các kiểu giày dành riêng cho mùa đông năm nay mà bất cứ ai đều phải sở hữu ít nhất một đôi:\r\n\r\n" +
								"Giày thể thao nam Biti’s Hunter Street Mid Americano ",
							CreatedAt = DateTime.Now,
							User = user, // Assign the retrieved user to the Blog's User property
                            TopicID = 1
						},
						new Blog
						{
							Name = "Happy New Year 2024"
						}
					});
					context.SaveChanges();
				}
			}
		}
	}
}
