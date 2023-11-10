using Bogus;
using Microsoft.AspNetCore.Identity;
using ShoeShop.Models;
using System;

namespace ShoeShop.Data.Seeder
{
    public class CustomerSeeder
    {
        public CustomerSeeder(IApplicationBuilder applicationBuilder)
        {
            CustomerAsync(applicationBuilder).Wait();
        }

        public async Task CustomerAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var faker = new Faker();

                for (int i = 0; i < 152; i++)
                {
                    var customerEmail = faker.Internet.Email();
                    var customerUserName = faker.Internet.UserName();

                    var existingCustomer = await userManager.FindByEmailAsync(customerEmail);

                    if (existingCustomer == null)
                    {
                        var newCustomer = new AppUser()
                        {
                            FullName = faker.Name.FullName(),
                            UserName = customerUserName,
                            Email = customerEmail,
                            EmailConfirmed = true,
                            PhoneNumber = faker.Phone.PhoneNumber(),
                            Status = faker.Random.Bool(),
                            Gender = faker.Random.Number(0, 1), // 0 for male, 1 for female
                            BirthDay = faker.Date.Past(),
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
