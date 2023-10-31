using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Models;

namespace ShoeShop.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Category> Categorys { get; set; }
		public DbSet<ShoeShop.Models.Color>? Color { get; set; }
		public DbSet<ShoeShop.Models.Brand>? Brand { get; set; }
	}
}
