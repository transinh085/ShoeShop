using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Controllers;
using ShoeShop.Data;
using ShoeShop.Models;
namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class ContactsController : Controller
	{
		private readonly AppDbContext _context;

		public ContactsController(AppDbContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index(string? searchInput)
		{
            if (searchInput != null)
            {

                var contactlist = await _context.Contacts.Where(c => c.Name.ToLower().Contains(searchInput.ToLower())).ToListAsync();

                if (contactlist == null)
                {
                    return NotFound(contactlist);
                }

                return View(contactlist);
            }

            return _context.Contacts != null ?
						  View(await _context.Contacts.ToListAsync()) :
						  Problem("Entity set 'AppDbContext.Sizes'  is null.");
		}
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var contactData = _context.Contacts.Where(b => b.IsDeleted == false).AsQueryable();
                //switch (sortColumn.ToLower())
                //{
                    
                //    case "Email":
                //        contactData = sortColumnDirection.ToLower() == "asc" ? contactData.OrderBy(o => o.Email) : contactData.OrderByDescending(o => o.Email);
                //        break;
                //    case "Name":
                //        contactData = sortColumnDirection.ToLower() == "asc" ? contactData.OrderBy(o => o.Name) : contactData.OrderByDescending(o => o.Name);
                //        break;
                //    case "CreatedAt":
                //        contactData = sortColumnDirection.ToLower() == "asc" ? contactData.OrderBy(o => o.CreatedAt) : contactData.OrderByDescending(o => o.CreatedAt);
                //        break;
                //    case "Message":
                //        contactData = sortColumnDirection.ToLower() == "asc" ? contactData.OrderBy(o => o.Message) : contactData.OrderByDescending(o => o.Message);
                //        break;
                //    default:
                //        contactData = contactData.OrderBy(o => o.Id);
                //        break;
                //}
                if (!string.IsNullOrEmpty(searchValue))
                {
                    contactData = contactData.Where(m => m.Name.Contains(searchValue));
                }
                recordsTotal = contactData.Count();
                var data = contactData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Read(int? id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (id !=null && contact != null)
            {
                if (contact.IsSeen == true) contact.IsSeen = false; else contact.IsSeen = true;
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
            }
            else
            { return NotFound(); }
            return RedirectToAction("Index");

        }

    }
}
