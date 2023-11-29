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
              return _context.Topics != null ? 
                          View(await _context.Topics.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Topic'  is null.");
        }

        // GET: Admin/Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
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
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
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
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Admin/Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Topics == null)
            {
                return Problem("Entity set 'AppDbContext.Topics'  is null.");
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                topic.IsDelete = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Delete successfully" });
        }

        private bool TopicExists(int id)
        {
          return (_context.Topics?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> GetTopics()
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
                var topicData = _context.Topics.Where(b => b.IsDelete == false).AsQueryable();
                switch (sortColumn.ToLower())
                {
                    case "id":
                        topicData = sortColumnDirection.ToLower() == "asc" ? topicData.OrderBy(o => o.Id) : topicData.OrderByDescending(o => o.Id);
                        break;
                    case "name":
                        topicData = sortColumnDirection.ToLower() == "asc" ? topicData.OrderBy(o => o.Name) : topicData.OrderByDescending(o => o.Name);
                        break;
                    default:
                        topicData = topicData.OrderBy(o => o.Id);
                        break;
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    topicData = topicData.Where(m => m.Name.Contains(searchValue));
                }
                recordsTotal = topicData.Count();
                var data = topicData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
