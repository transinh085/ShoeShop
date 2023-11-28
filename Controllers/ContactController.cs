using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels;

namespace ShoeShop.Controllers
{
	public class ContactController : Controller
	{
		private readonly AppDbContext _context;

		public ContactController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> SubmitContact([Bind("Name, Email, Message")] ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				Contact contact = new Contact()
				{
					Name = model.Name,
					Email = model.Email,
					Message = model.Message,
				};
				_context.Add(contact);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index");
			}

			return RedirectToAction("Index");
		}

	}
}
