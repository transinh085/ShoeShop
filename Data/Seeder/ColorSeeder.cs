using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class ColorSeeder
    {
        public ColorSeeder(IApplicationBuilder applicationBuilder)
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
    }
}
