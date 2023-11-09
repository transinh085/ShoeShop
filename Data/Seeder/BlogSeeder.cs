using ShoeShop.Models;
namespace ShoeShop.Data.Seeder
{
    public class BlogSeeder
    {
        public BlogSeeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Blogs.Any())
                {
                    context.Blogs.AddRange(new List<Blog>()
                    {
                        new Blog
                        {
                            Name = "Sales 2024"
                        },
                        new Blog
                        {
                            Name = "Happy New Year 2024"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
