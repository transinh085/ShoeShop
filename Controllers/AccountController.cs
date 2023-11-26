using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Data.Enum;
using ShoeShop.Models;
using ShoeShop.ViewModels;

namespace ShoeShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Profile()
		{
            
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileViewModel model)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var existingUserWithUpdatedUsername = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserWithUpdatedUsername != null && existingUserWithUpdatedUsername.Id != currentUser.Id)
                {
                    return BadRequest(new { errors = new[] { new { key = "UserName", value = "Username is already taken." } } });
                }

                // Check if the updated email is unique
                var existingUserWithUpdatedEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserWithUpdatedEmail != null && existingUserWithUpdatedEmail.Id != currentUser.Id)
                {
                    return BadRequest(new { errors = new[] { new { key = "Email", value = "Email is already taken." } } });
                }
                currentUser.FullName = model.FullName;
                currentUser.UserName = model.UserName;
                currentUser.Email = model.Email;
                currentUser.PhoneNumber = model.Phone;
                currentUser.Gender = model.Gender;

                if (model.ImageFile != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine("wwwroot/img/users", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    currentUser.Image = uniqueFileName;
                }

                var updateResult = await _userManager.UpdateAsync(currentUser);

                if (updateResult.Succeeded)
                {
                    return Ok(currentUser);
                }
                else
                {
                    var errors = updateResult.Errors.Select(error => new { key = error.Code, value = error.Description });
                    return BadRequest(new { errors });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while processing the request", Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    return BadRequest(new { errors = new[] { new { key = "User", value = "User not found." } } });
                }

                // Change the password without requiring the old password
                var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Password changed successfully." });
                }
                else
                {
                    var errors = result.Errors.Select(error => new { key = error.Code, value = error.Description });
                    return BadRequest(new { errors });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred while processing the request", Error = ex.Message });
            }
        }

        public IActionResult Orders()
        {
            return View();
        }

        public async Task<IActionResult> GetOrders(string query, string dateStart, string dateEnd)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var draw = int.Parse(Request.Form["draw"].FirstOrDefault());
                var skip = int.Parse(Request.Form["start"].FirstOrDefault());
                var pageSize = int.Parse(Request.Form["length"].FirstOrDefault());
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var orderData = _context.Orders
                    .Where(o => o.AppUserId == currentUser.Id)
                    .Select(o => new
                    {
                        o.Id,
                        PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                        ShippingMethod = o.ShippingMethod.Name,
                        Total = o.SubTotal + o.ShippingFee,
                        o.OrderStatus,
                        o.CreatedAt,
                        Customer = o.AppUser.FullName,
                    })
                    .OrderByDescending(o => o.Id)
                    .AsQueryable();

                switch (sortColumn.ToLower())
                {
                    case "id":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.Id) : orderData.OrderByDescending(o => o.Id);
                        break;
                    case "total":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.Total) : orderData.OrderByDescending(o => o.Total);
                        break;
                    case "submited":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.CreatedAt) : orderData.OrderByDescending(o => o.CreatedAt);
                        break;
                    default:
                        orderData = orderData.OrderBy(o => o.Id);
                        break;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    orderData = orderData.Where(o => o.Id.ToString().Contains(query));
                }

                if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                {
                    var startDate = DateTime.Parse(dateStart);
                    var endDate = DateTime.Parse(dateEnd).AddDays(1);
                    orderData = orderData.Where(o => o.CreatedAt >= startDate && o.CreatedAt < endDate);
                }


                var recordsTotal = await orderData.CountAsync();
                var data = await orderData.Skip(skip).Take(pageSize).ToListAsync();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("/account/orders/detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            // Lấy thông tin người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);

            var checkOrderUser = _context.Orders
                .Where(o => o.Id == id && o.AppUserId == currentUser.Id)
                .FirstOrDefault();

            if (checkOrderUser == null)
            {
                return NotFound();
            }

            // Lấy thông tin đơn hàng
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                    ShippingMethod = o.ShippingMethod.Name,
                    o.SubTotal,
                    o.ShippingFee,
                    o.Description,
                    o.PaymentStatus,
                    o.OrderStatus,
                    o.Address,
                    o.CreatedAt,
                    Customer = o.AppUser,
                    Details = o.Details.Select(p => new
                    {
                        VariantSizeId = p.VariantSizeId,
                        ProductSlug = p.VariantSize.Variant.Product.Slug,
                        ProductId = p.VariantSize.Variant.Product.Id,
                        Name = p.VariantSize.Variant.Product.Name,
                        Size = p.VariantSize.Size.Name,
                        Color = p.VariantSize.Variant.Color.Name,
                        Stock = p.VariantSize.Quantity,
                        p.Price,
                        p.Quantity,
                    }).ToList()
                }).FirstOrDefault();

            if (order == null)
            {
                // Đơn hàng không tồn tại
                return NotFound();
            }

            // Truyền thông tin đơn hàng đến view
            ViewBag.Order = order;
            return View();
        }

        [HttpPost]
        [Route("/account/orders/canceled/{id}")]
        public async Task<IActionResult> Canceled(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var checkOrderUser = _context.Orders
                .Where(o => o.Id == id && o.AppUserId == currentUser.Id)
                .FirstOrDefault();

            if (checkOrderUser == null)
            {
                return Ok("Not found");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Canceled;
                _context.SaveChangesAsync();
                return Json(new { status = "Canceled" });
            }
            return Json(new { status = "Not found order id" });
        }


        public IActionResult Address()
        {
            return View();
        }

    }
}
