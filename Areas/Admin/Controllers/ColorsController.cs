using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class ColorsController : Controller
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Colors
        public async Task<IActionResult> Index()
        {
              return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetColors()
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
                var colorData = _context.Colors.Where(b => b.IsDelete == false).AsQueryable();
                switch (sortColumn.ToLower())
                {
                    case "id":
                        colorData = sortColumnDirection.ToLower() == "asc" ? colorData.OrderBy(o => o.Id) : colorData.OrderByDescending(o => o.Id);
                        break;
                    case "name":
                        colorData = sortColumnDirection.ToLower() == "asc" ? colorData.OrderBy(o => o.Name) : colorData.OrderByDescending(o => o.Name);
                        break;
                    default:
                        colorData = colorData.OrderBy(o => o.Id);
                        break;
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    colorData = colorData.Where(m => m.Name.Contains(searchValue));
                }
                recordsTotal = colorData.Count();
                var data = colorData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Admin/Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Color color)
        {
            if (ModelState.IsValid)
            {
                _context.Add(color);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(color);
        }

        // GET: Admin/Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colors == null)
            {
                return NotFound();
            }

            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            return View(color);
        }

        // POST: Admin/Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Color color)
        {
            if (id != color.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(color);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(color.Id))
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
            return View(color);
        }

        // GET: Admin/Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Colors == null)
            {
                return NotFound();
            }

            var color = await _context.Colors
				.FirstOrDefaultAsync(m => m.Id == id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // POST: Admin/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Colors == null)
            {
                return Problem("Entity set 'AppDbContext.Color'  is null.");
            }

            var color = await _context.Colors.FindAsync(id);
            if (color != null)
            {
                color.IsDelete = true;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new { message = "Delete successfully" });
        }

        private bool ColorExists(int id)
        {
          return (_context.Colors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
