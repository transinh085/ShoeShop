using Microsoft.AspNetCore.Identity;
using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class CustomerSeeder
    {
        public CustomerSeeder(IApplicationBuilder applicationBuilder) {
            CustomerAsync(applicationBuilder).Wait();
        }

        public async Task CustomerAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
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
                            FullName = $"hgbaodev {i + 1}",
                            UserName = customerUserName,
                            Email = customerEmail,
                            EmailConfirmed = true,
                            PhoneNumber = "0123456789",
                            ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4",
                            Status = true,
                            Gender = 0,
                            BirthDay = DateTime.Now,
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
