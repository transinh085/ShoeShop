using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.ViewModels.Authentication;
using System.Diagnostics;
using ShoeShop.Services;

namespace ShoeShop.Controllers
{
    public class AuthenticationController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly AppDbContext _context;
		private readonly ISendMailService _mailService;

		public AuthenticationController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			AppDbContext context, ISendMailService mailService)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_mailService = mailService;
		}

		public IActionResult Signin()
        {
            if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
            var response = new SigninViewModel();
            return View(response);
        }

		[HttpPost]
		public async Task<IActionResult> Signin(SigninViewModel loginViewModel, string? returnUrl)
		{
			if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
			if (!ModelState.IsValid) return View(loginViewModel);

			var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

			if (user != null)
			{
				//User is found, check password
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
				if (passwordCheck)
				{
					//Password correct, sign in
					var result = await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
					Console.WriteLine(result);
					if (result.Succeeded)
					{
						if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
						return RedirectToAction("Index", "Home");
					} 
					if(result.IsLockedOut)
					{
                        return View("Lockout");
					}
				}
				//Password is incorrect
				TempData["Error"] = "Wrong credentials. Please try again";
				return View(loginViewModel);
			}
			//User not found
			TempData["Error"] = "Wrong credentials. Please try again";
			return View(loginViewModel);
		}

		[HttpGet]
		public IActionResult Register()
		{
			var response = new RegisterViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid) return View(registerViewModel);

			var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerViewModel);
			}
			user = await _userManager.FindByNameAsync(registerViewModel.UserName);
			if (user != null)
			{
				TempData["Error"] = "This username address is already in use";
				return View(registerViewModel);
			}

			var newUser = new AppUser()
			{
				FullName = registerViewModel.FullName,
				UserName = registerViewModel.UserName,
				Email = registerViewModel.EmailAddress,
			};
			if (newUser == null)
			{
				TempData["Error"] = "Create account errors";
				return View(registerViewModel);
			}
			var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
			if (newUserResponse.Succeeded)
				await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					TempData["Error"] = "Account no found. Please try again";
					return View(model);
				}
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);
				var callbackurl = Url.Action("ResetPassword", "Authentication", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

				await _mailService.SendEmailAsync(model.Email, "Reset Email Confirmation", "Please reset email by going to this " +
					"<a href=\"" + callbackurl + "\">link</a>");
				return RedirectToAction("ForgotPasswordConfirmation");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ResetPassword(string code = null)
		{
			return code == null ? View("Error") : View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
				if (user == null)
				{
					TempData["Error"] = "User not found";
					return View(resetPasswordViewModel);
				}
				var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("ResetPasswordConfirmation");
				}
			}
			return View(resetPasswordViewModel);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
