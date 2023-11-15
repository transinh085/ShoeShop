using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Controllers;
using ShoeShop.Data;
using ShoeShop.Models;
namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
	{
		private readonly AppDbContext _context;

		public ContactsController(AppDbContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index()
		{
			return _context.Contacts != null ?
						  View(await _context.Contacts.ToListAsync()) :
						  Problem("Entity set 'AppDbContext.Sizes'  is null.");
		}

        [HttpPost, ActionName("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            Console.WriteLine(contact.Name);
            if (contact == null) return NotFound();

            ViewBag.Contact = contact;

            return View();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.IsDeleted = true;
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            return Ok(new { message = "Delete successfully" });
        }
    }
}
