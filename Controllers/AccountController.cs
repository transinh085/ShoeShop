using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Data;
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

    }
}
