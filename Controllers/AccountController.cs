using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Data.Enum;
using ShoeShop.Hubs;
using ShoeShop.Models;
using ShoeShop.ViewModels;
using System.Security.Claims;

namespace ShoeShop.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<OrderHub> _orderHubContext;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, AppDbContext context, IHubContext<OrderHub> orderHubContext)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _orderHubContext = orderHubContext;
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

        public async Task<IActionResult> GetOrders(string query, int status)
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

                if (status != -2)
                {
                    var orderStatusFilter = (OrderStatus)status;
                    orderData = orderData.Where(o => o.OrderStatus == orderStatusFilter);
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
                await _orderHubContext.Clients.All.SendAsync("ReceiveOrderUpdate");

                return Json(new { status = "Canceled" });
            }
            return Json(new { status = "Not found order id" });
        }


        public IActionResult Address()
        {
            return View();
        }

        public async Task<IActionResult> GetAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var addresses = await _context.Addresses.Where(a => a.AppUserId == userId && !a.IsDelete).ToListAsync();
            return Ok(addresses);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_context.Colors == null) return Problem("Entity set 'AppDbContext.Color'  is null.");

            var color = await _context.Addresses.FindAsync(id);
            if (color != null) color.IsDelete = true;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Delete successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressViewModel addressViewModel)
        {
            try
            {
                if(addressViewModel.IsDefault)
                {
                    var defaultAddress = _context.Addresses.Where(a => a.IsDefault).FirstOrDefault();
                    if (defaultAddress != null) defaultAddress.IsDefault = false;
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var newAddress = new Address
                {
                    FullName = addressViewModel.FullName,
                    Email = addressViewModel.Email,
                    Phone = addressViewModel.Phone,
                    SpecificAddress = addressViewModel.SpecificAddress,
                    AppUserId = userId,
                    IsDefault = addressViewModel.IsDefault,
                };

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();
                return Ok(newAddress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }


        [HttpPut]
        public async Task<IActionResult> EditAddress(int id, [FromBody] AddressViewModel addressViewModel)
        {
            try
            {
                // Lấy địa chỉ cần chỉnh sửa từ cơ sở dữ liệu
                var addressToUpdate = await _context.Addresses.FindAsync(id);

                if (addressToUpdate == null)
                {
                    return NotFound(new { message = "Address not found" });
                }

                // Nếu địa chỉ đang được chỉnh sửa là địa chỉ mặc định, hãy tìm địa chỉ mặc định hiện tại và cập nhật nó
                if (addressViewModel.IsDefault)
                {
                    var currentDefaultAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.IsDefault);

                    if (currentDefaultAddress != null && currentDefaultAddress.Id != id)
                    {
                        currentDefaultAddress.IsDefault = false;
                    }
                }

                // Cập nhật thông tin của địa chỉ
                addressToUpdate.FullName = addressViewModel.FullName;
                addressToUpdate.Email = addressViewModel.Email;
                addressToUpdate.Phone = addressViewModel.Phone;
                addressToUpdate.SpecificAddress = addressViewModel.SpecificAddress;
                addressToUpdate.IsDefault = addressViewModel.IsDefault;

                await _context.SaveChangesAsync();

                return Ok(addressToUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}
