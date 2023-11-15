using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

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

		public async Task<IActionResult> SubmitContact(string name, string email, string message)
		{
			Contact contact = new Contact()
			{
				Name = name,
				Email = email,
				Message = message,
			};
			_context.Add(contact);
			await _context.SaveChangesAsync();
			
			return RedirectToAction("Index");
		}
	}
}
