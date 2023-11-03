using Microsoft.AspNetCore.Identity;
using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class RoleSeeder
    {
        public RoleSeeder(IApplicationBuilder applicationBuilder) {
            RolesAsync(applicationBuilder).Wait(); 
        }

        public async Task RolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
            }
        }
    }
}
