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
        public async Task<IActionResult> AddCustomer([Bind("FullName,UserName,Email,PhoneNumber,BirthDay,Password,Gender,Status")] CustomerViewModel customerView)
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
                    FullName = customerView.FullName,
                    UserName = customerView.UserName,
                    Email = customerView.Email,
                    EmailConfirmed = true,
                    PhoneNumber = customerView.PhoneNumber,
                    ProfileImageUrl = "https://avatars.githubusercontent.com/u/120194990?v=4",
                    Status = customerView.Status,
                    Gender = customerView.Gender,
                    BirthDay = customerView.BirthDay,
                };
                var newUserResponse = await userManager.CreateAsync(newUser, customerView.Password);

                if (newUserResponse.Succeeded)
                    await userManager.AddToRoleAsync(newUser, UserRoles.Customer);
                user = await userManager.FindByEmailAsync(customerView.Email);
                return Ok(user);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors); 
            }
        }



    }
}
