using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
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
        public IActionResult GetCustomerList(int page = 1, int pageSize = 10, string query = "")
        {
            // Lấy danh sách người dùng
            var users = userManager.Users.ToList();

            // Sử dụng LINQ để lọc danh sách người dùng có quyền "Customer"
            var customerUsers = users
                .Where(u => userManager.IsInRoleAsync(u, UserRoles.Customer).Result)
                .Where(u => u.IsDeleted == false)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.UserName,
                    u.Gender,
                    u.BirthDay,
                    u.Email,
                    u.PhoneNumber,
                    u.Status,
                    u.JoinTime,
                })
                .OrderByDescending(u => u.JoinTime)
                .ToList();

            if(!string.IsNullOrEmpty(query))
            {
                customerUsers = customerUsers.Where(u =>
                    u.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();  
            }

            if (page < 1)
            {
                page = 1;
            }

            // Tính tổng số trang dựa trên số lượng khách hàng và kích thước trang
            int totalItems = customerUsers.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Lấy danh sách khách hàng của trang hiện tại
            var currentPageCustomers = customerUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Tạo một đối tượng chứa thông tin trang và danh sách khách hàng
            var result = new
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Result = currentPageCustomers
            };

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> AddCustomer([Bind("FullName,UserName,Email,PhoneNumber,BirthDay,Password,Gender,Status")] CustomerViewModel customerView)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(customerView.Email);
                var checkUserName = await userManager.FindByNameAsync(customerView.UserName);
                if (customerView.Status != true)
                {
                    return BadRequest(new { error = "Status must be true." });
                }
                if (user != null)
                {
                    return Conflict(new { error = "Email address is already in use." });
                }
                if (checkUserName != null)
                {
                    return Conflict(new { error = "User name is already in use." });
                }
                var newUser = new AppUser()
                {
                    FullName = customerView.FullName,
                    UserName = customerView.UserName,
                    Email = customerView.Email,
                    EmailConfirmed = true,
                    PhoneNumber = customerView.PhoneNumber,
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

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(string id, [Bind("FullName,UserName,Email,PhoneNumber,BirthDay,Password,Gender,Status")] CustomerViewModel customerView)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new { error = "User not found." });
                }

                if (customerView.Status != true)
                {
                    return BadRequest(new { error = "Status must be true." });
                }

                // Kiểm tra xem có email khác đã sử dụng không, ngoại trừ người dùng hiện tại
                var userWithEmail = await userManager.FindByEmailAsync(customerView.Email);
                if (userWithEmail != null && userWithEmail.Id != id)
                {
                    return Conflict(new { error = "Email address is already in use." });
                }

                // Cập nhật thông tin người dùng
                existingUser.FullName = customerView.FullName;
                existingUser.UserName = customerView.UserName;
                existingUser.Email = customerView.Email;
                existingUser.PhoneNumber = customerView.PhoneNumber;
                existingUser.Status = customerView.Status;
                existingUser.Gender = customerView.Gender;
                existingUser.BirthDay = customerView.BirthDay;


                var updateResult = await userManager.UpdateAsync(existingUser);

                if (updateResult.Succeeded)
                {
                    return Ok(existingUser);
                }
                else
                {
                    var errors = updateResult.Errors.Select(error => error.Description);
                    return BadRequest(new { error = "User update failed.", errors });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var existingUser = await userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound(new { error = "User not found." });
            }

            // Cập nhật trạng thái IsDeleted thành true để đánh dấu người dùng đã bị xoá
            existingUser.IsDeleted = true;

            var updateResult = await userManager.UpdateAsync(existingUser);

            if (updateResult.Succeeded)
            {
                return Ok(new { message = "User deleted successfully." });
            }
            else
            {
                var errors = updateResult.Errors.Select(error => error.Description);
                return BadRequest(new { error = "User deletion failed.", errors });
            }
        }

    }
}
