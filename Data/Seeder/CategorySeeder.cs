using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class CategorySeeder
    {
        public CategorySeeder(IApplicationBuilder applicationBuilder)
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
    }
}
