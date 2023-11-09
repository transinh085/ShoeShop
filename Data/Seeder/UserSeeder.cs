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
                var adminUser = await userManager.FindByEmailAsync("transinh085@gmail.com");
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        FullName = "Trần Nhật Sinh",
                        UserName = "transinh085",
                        Email = "transinh085@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "0123456789",
                        ProfileImageUrl = "https://avatars.githubusercontent.com/u/45101901?v=4",
                        Status = true,
                        Gender = 0,
                        BirthDay = DateTime.Now,
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                var appUser = await userManager.FindByEmailAsync("hgbao2k3@gmail.com");
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        FullName = "Hoàng Gia Bảo",
                        UserName = "hgbaodev",
                        Email = "hgbao2k3@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "0123456789",
                        ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4",
                        Status = true,
                        Gender = 0,
                        BirthDay = DateTime.Now,
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
                }

                var appUser1 = await userManager.FindByEmailAsync("andinh1443@gmail.com");
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        FullName = "Đinh Ngọc Ân",
                        UserName = "andinh1443",
                        Email = "andinh1443@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "0123456789",
                        ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4",
                        Status = true,
                        Gender = 0,
                        BirthDay = DateTime.Now,
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
                }
            }
        }
    }
}
