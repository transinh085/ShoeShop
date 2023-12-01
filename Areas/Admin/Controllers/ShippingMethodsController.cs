using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class ShippingMethodsController : Controller
    {
        private readonly AppDbContext _context;

        public ShippingMethodsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ShippingMethods
        public async Task<IActionResult> Index()
        {
              return View();
        }

        public async Task<IActionResult> GetShippingMethod()
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
                var sizeData = _context.ShippingMethods.Where(p => p.IsDelete == false).AsQueryable();
                switch (sortColumn.ToLower())
                {
                    case "id":
                        sizeData = sortColumnDirection.ToLower() == "asc" ? sizeData.OrderBy(o => o.Id) : sizeData.OrderByDescending(o => o.Id);
                        break;
                    case "cost":
                        sizeData = sortColumnDirection.ToLower() == "asc" ? sizeData.OrderBy(o => o.Cost) : sizeData.OrderByDescending(o => o.Cost);
                        break;
                    case "description":
                        sizeData = sortColumnDirection.ToLower() == "asc" ? sizeData.OrderBy(o => o.Description) : sizeData.OrderByDescending(o => o.Description);
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

        // GET: Admin/ShippingMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ShippingMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Cost,isDelete")] ShippingMethod shippingMethod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingMethod);
        }

        // GET: Admin/ShippingMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShippingMethods == null)
            {
                return NotFound();
            }

            var shippingMethod = await _context.ShippingMethods.FindAsync(id);
            if (shippingMethod == null)
            {
                return NotFound();
            }
            return View(shippingMethod);
        }

        // POST: Admin/ShippingMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Cost,isDelete")] ShippingMethod shippingMethod)
        {
            if (id != shippingMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingMethodExists(shippingMethod.Id))
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
            return View(shippingMethod);
        }

        // POST: Admin/ShippingMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.ShippingMethods == null)
            {
                return Problem("Entity set 'AppDbContext.ShippingMethod'  is null.");
            }
            var shippingMethod = await _context.ShippingMethods.FindAsync(id);
            if (shippingMethod != null)
            {
                shippingMethod.IsDelete = true;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new {message = "Delete successfully"});
        }

        private bool ShippingMethodExists(int id)
        {
          return (_context.ShippingMethods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
