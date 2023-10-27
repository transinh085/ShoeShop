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
            if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
            var response = new SigninViewModel();
            return View(response);
        }

		[HttpPost]
		public async Task<IActionResult> Signin(SigninViewModel loginViewModel)
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
					var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
					if (result.Succeeded)
					{
						Console.WriteLine("success");
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

			var newUser = new AppUser()
			{
				UserName = registerViewModel.UserName,
				Email = registerViewModel.EmailAddress,
			};
			var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
			Console.WriteLine("add: "+newUserResponse.ToString());
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


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
