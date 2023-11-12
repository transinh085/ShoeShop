using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;

namespace ShoeShop.Areas.Admin.Controllers
{
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
              return _context.ShippingMethods != null ? 
                          View(await _context.ShippingMethods.Where(c => c.IsDelete == false).ToListAsync()) :
                          Problem("Entity set 'AppDbContext.ShippingMethod'  is null.");
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
