using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
              return View();
        }

		[HttpPost]
		public async Task<IActionResult> GetCategories()
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
				var categoryData = _context.Categories.Where(b => b.IsDelete == false).AsQueryable();
                switch (sortColumn.ToLower())
                {
                    case "id":
                        categoryData = sortColumnDirection.ToLower() == "asc" ? categoryData.OrderBy(o => o.Id) : categoryData.OrderByDescending(o => o.Id);
                        break;
                    case "name":
                        categoryData = sortColumnDirection.ToLower() == "asc" ? categoryData.OrderBy(o => o.Name) : categoryData.OrderByDescending(o => o.Name);
                        break;
                    default:
                        categoryData = categoryData.OrderBy(o => o.Id);
                        break;
                }
                if (!string.IsNullOrEmpty(searchValue))
				{
					categoryData = categoryData.Where(m => m.Name.Contains(searchValue));
				}
				recordsTotal = categoryData.Count();
				var data = categoryData.Skip(skip).Take(pageSize).ToList();
				var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
				return Ok(jsonData);
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		// GET: Admin/Categories/Create
		public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDelete = true;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new { message = "Delete successfully" });
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
