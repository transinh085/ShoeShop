using ShoeShop.Models;
namespace ShoeShop.Data.Seeder
{
    public class TopicSeeder
    {
        public TopicSeeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Topics.Any())
                {
                    context.Topics.AddRange(new List<Topic>()
                    {
                        new Topic
                        {
                            Name = "Trending"
                        },
                        new Topic
                        {
                            Name = "Product"
                        },
                        new Topic
                        {
                            Name = "Sale"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
