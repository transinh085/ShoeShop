using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class SizesController : Controller
    {
        private readonly AppDbContext _context;

        public SizesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Sizes
        public async Task<IActionResult> Index()
        {
              return View();
        }

        public async Task<IActionResult> GetSizes()
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
                var sizeData = _context.Sizes.Where(p => p.IsDelete == false).AsQueryable();
                switch (sortColumn.ToLower())
                {
                    case "id":
                        sizeData = sortColumnDirection.ToLower() == "asc" ? sizeData.OrderBy(o => o.Id) : sizeData.OrderByDescending(o => o.Id);
                        break;
                    case "name":
                        sizeData = sortColumnDirection.ToLower() == "asc" ? sizeData.OrderBy(o => o.Name) : sizeData.OrderByDescending(o => o.Name);
                        break;
                    default:
                        sizeData = sizeData.OrderBy(o => o.Id);
                        break;
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    sizeData = sizeData.Where(m => m.Name.Contains(searchValue));
                }
                recordsTotal = sizeData.Count();
                var data = sizeData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        

        // GET: Admin/Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Size size)
        {
            if (ModelState.IsValid)
            {
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        // GET: Admin/Sizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Admin/Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.Id))
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
            return View(size);
        }

        // POST: Admin/Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sizes == null)
            {
                return Problem("Entity set 'AppDbContext.Sizes'  is null.");
            }
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                size.IsDelete = true;
            }

            await _context.SaveChangesAsync();
            return Json(new {message = "Delete size successful !"});
        }

        private bool SizeExists(int id)
        {
          return (_context.Sizes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
