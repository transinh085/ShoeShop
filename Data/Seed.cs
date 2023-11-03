using Microsoft.AspNetCore.Identity;
using ShoeShop.Models;

namespace ShoeShop.Data
{
	public class Seed
	{
		public static void SeedData(IApplicationBuilder applicationBuilder)
		{
			Seed.CategorySeeder(applicationBuilder);
			Seed.ColorSeeder(applicationBuilder);
			Seed.BrandSeeder(applicationBuilder);
		}

		public static void CategorySeeder(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				if (!context.Categories.Any())
				{
					context.Categories.AddRange(new List<Category>()
					{
						new Category
						{
							Name = "Sport"
						},
						new Category
						{
							Name = "Jogging"
						},
						new Category
						{
							Name = "Running"
						},
						new Category
						{
							Name = "Basketball"
						},
						new Category
						{
							Name = "Hiking"
						}
					});
					context.SaveChanges();
				}
			}
		}

        public static void BrandSeeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Brands.Any())
                {
                    context.Brands.AddRange(new List<Brand>()
                    {
                        new Brand
                        {
                            Name = "Adidas",
                        },
                        new Brand
                        {
                            Name = "Nike"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static void ColorSeeder(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				if (!context.Colors.Any())
				{
					context.Colors.AddRange(new List<Color>()
					{
						new Color
						{
							Name = "White"
						},
						new Color
						{
							Name = "Black"
						}
					});
					context.SaveChanges();
				}
			}
		}

		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				string adminUserEmail = "transinh085@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new AppUser()
					{
						UserName = "transinh085",
						Email = adminUserEmail,
						EmailConfirmed = true,
						PhoneNumber = "0123456789",
                        ProfileImageUrl = "https://avatars.githubusercontent.com/u/45101901?v=4"
                    };
					await userManager.CreateAsync(newAdminUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				string appUserEmail = "hgbao2k3@gmail.com";
				var appUser = await userManager.FindByEmailAsync(appUserEmail);
				if (appUser == null)
				{
					var newAppUser = new AppUser()
					{
						UserName = "hgbaodev",
						Email = appUserEmail,
						EmailConfirmed = true,
                        PhoneNumber = "0123456789",
                        ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4"
                    };
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
				}

                // Tạo 20 tài khoản khách hàng
                for (int i = 0; i < 20; i++)
                {
                    var customerEmail = $"customer{i + 1}@example.com";
                    var customerUserName = $"customer{i + 1}";

                    var existingCustomer = await userManager.FindByEmailAsync(customerEmail);

                    if (existingCustomer == null)
                    {
                        var newCustomer = new AppUser()
                        {
                            UserName = customerUserName,
                            Email = customerEmail,
                            EmailConfirmed = true,
                            PhoneNumber = "0123456789",
                            ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4"
                        };

                        // Tạo tài khoản khách hàng và thêm vào vai trò "Customer"
                        await userManager.CreateAsync(newCustomer, "Coding@1234?");
                        await userManager.AddToRoleAsync(newCustomer, UserRoles.Customer);
                    }
                }
            }
		}
	}
}
