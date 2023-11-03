using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
	{
		private readonly UserManager<AppUser> userManager;

		public CustomersController(UserManager<AppUser> userManager)
		{
			this.userManager = userManager;
		}

		// GET: Admin/Customers
		public async Task<IActionResult> Index()
		{
            var users = await userManager.Users.ToListAsync();

            // Lọc ra danh sách người dùng có quyền "Customer"
            var customerUsers = users.Where(u => userManager.IsInRoleAsync(u, UserRoles.Customer).Result).ToList();

            return View(customerUsers);
        }

    }
}
