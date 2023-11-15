using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
    public class BrandSeeder
    {
        public BrandSeeder(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Brands.Any())
                {
                    context.Brands.AddRange(new List<Brand>()
                    {
                        new Brand
                        {
                            Name = "Adidas",
                        },
                        new Brand
                        {
                            Name = "Nike",
                        },
						new Brand
						{
							Name = "Bitis",
						},
                        new Brand
                        {
                            Name = "Ananas",
                        },
					});
                    context.SaveChanges();
                }
            }
        }
    }
}
