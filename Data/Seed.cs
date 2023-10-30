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
					};
					await userManager.CreateAsync(newAppUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.Customer);
				}
			}
		}
	}
}
