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
    public class TopicsController : Controller
    {
        private readonly AppDbContext _context;

        public TopicsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Topics
        public async Task<IActionResult> Index()
        {
              return _context.Topic != null ? 
                          View(await _context.Topic.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Topic'  is null.");
        }

        // GET: Admin/Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Admin/Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Admin/Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Admin/Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.Id))
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
            return View(topic);
        }

        // GET: Admin/Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Topic == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Topic == null)
            {
                return Problem("Entity set 'AppDbContext.Topic'  is null.");
            }
            var topic = await _context.Topic.FindAsync(id);
            if (topic != null)
            {
                _context.Topic.Remove(topic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
          return (_context.Topic?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
