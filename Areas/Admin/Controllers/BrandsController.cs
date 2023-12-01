using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	//[Authorize]
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

		// GET: Admin/Brands
		public async Task<IActionResult> Index()
		{
			return _context.Brands != null ?
						View(await _context.Brands.Where(c => c.IsDelete == false).ToListAsync()) :
						Problem("Entity set 'AppDbContext.Brands'  is null.");
		}

        [HttpPost]
        public async Task<IActionResult> GetBrands()
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
                var categoryData = _context.Brands.Where(b => b.IsDelete == false).AsQueryable();
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

        // GET: Admin/Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'AppDbContext.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                brand.IsDelete = true ;
            }

            await _context.SaveChangesAsync();
            return Ok(new {message = "Success delete brand!"});
        }

        private bool BrandExists(int id)
        {
          return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
