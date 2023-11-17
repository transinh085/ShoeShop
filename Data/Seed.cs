using ShoeShop.Data.Seeder;
namespace ShoeShop.Data
{
	public class Seed
	{
		public static void SeedData(IApplicationBuilder applicationBuilder)
		{
			new RoleSeeder(applicationBuilder);
			new UserSeeder(applicationBuilder);
			new CustomerSeeder(applicationBuilder);
			new CategorySeeder(applicationBuilder);
			new BrandSeeder(applicationBuilder);
			new ColorSeeder(applicationBuilder);
			new SizeSeeder(applicationBuilder);
			new TopicSeeder(applicationBuilder);
			//new BlogSeeder(applicationBuilder);
			new ProductSeeder(applicationBuilder);
			new ShippingMethodSeeder(applicationBuilder);
		}
	}
}
