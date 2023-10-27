using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.ViewModels.Authentication;
using System.Diagnostics;

namespace ShoeShop.Controllers
{
    public class AuthenticationController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly AppDbContext _context;

		public AuthenticationController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			AppDbContext context)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public IActionResult Signin()
        {
			var response = new SigninViewModel();
			return View(response);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Signin(SigninViewModel loginViewModel)
		{
			if (!ModelState.IsValid) return View(loginViewModel);

			var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

			if (user != null)
			{
				//User is found, check password
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
				if (passwordCheck)
				{
					//Password correct, sign in
					var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
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


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
