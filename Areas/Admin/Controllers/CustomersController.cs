using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels;
using ShoeShop.ViewModels.Authentication;

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
		public IActionResult Index()
		{
            return View();
        }

        [HttpGet, ActionName("allCustomers")]
        public async Task<IActionResult> GetCustomerList()
        {
            // Lấy danh sách người dùng
            var users = await userManager.Users.ToListAsync();

            // Sử dụng LINQ để lọc danh sách người dùng có quyền "Customer"
            var customerUsers = users.Where(u => userManager.IsInRoleAsync(u, UserRoles.Customer).Result).ToList();

            // Sắp xếp danh sách theo JoinTime (thời gian gia nhập) giảm dần
            customerUsers = customerUsers.OrderByDescending(u => u.JoinTime).ToList();

            return Ok(customerUsers);
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomer([Bind("UserName,Email,PhoneNumber,BirthDay,Password,Gender,Status")] CustomerViewModel customerView)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(customerView.Email);
                if (customerView.Status != true)
                {
                    return BadRequest(new { error = "Status must be true." });
                }
                if (user != null)
                {
                    return Conflict(new { error = "Email address is already in use." });
                }
                var newUser = new AppUser()
                {
                    UserName = customerView.UserName,
                    Email = customerView.Email,
                };
                var newUserResponse = await userManager.CreateAsync(newUser, customerView.Password);
                return Ok(customerView);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors); 
            }
        }



    }
}
