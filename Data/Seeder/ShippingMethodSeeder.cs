using ShoeShop.Models;

namespace ShoeShop.Data.Seeder
{
	public class ShippingMethodSeeder
	{
		public ShippingMethodSeeder(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				if (!context.ShippingMethods.Any())
				{
					context.ShippingMethods.AddRange(new List<ShippingMethod>()
					{
						new ShippingMethod
						{
							Name = "Standard Delivery",
							Description = "4-5 working days",
							Cost = 0
						},
						new ShippingMethod
						{
							Name = "Express Delivery",
							Description = "1-2 working days",
							Cost = 4
						}
					});
					context.SaveChanges();
				}
			}
		}
	}
}
