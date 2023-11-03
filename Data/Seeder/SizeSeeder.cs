using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class SizeSeeder
    {
        public SizeSeeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Sizes.Any())
                {
                    context.Sizes.AddRange(new List<Size>()
            {
                new Size
                {
                    Name = "36"
                },
                new Size
                {
                    Name = "37"
                },
                new Size
                {
                    Name = "38"
                },
                new Size
                {
                    Name = "39"
                },
                new Size
                {
                    Name = "40"
                },
                new Size
                {
                    Name = "41"
                },
                new Size
                {
                    Name = "42"
                },
                new Size
                {
                    Name = "43"
                }
            });
                    context.SaveChanges();
                }
            }
        }
    }
}
