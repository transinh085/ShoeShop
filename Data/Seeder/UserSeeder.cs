using Microsoft.AspNetCore.Identity;
using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class UserSeeder
    {
        public UserSeeder(IApplicationBuilder applicationBuilder) {
            UsersAsync(applicationBuilder).Wait();
        }
        public async Task UsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
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
