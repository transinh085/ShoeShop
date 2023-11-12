using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using ShoeShop.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ShoeShop.ViewModels;
using ShoeShop.Data.Seeder;
namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Blogs.Include(b => b.Topic);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TopicID"] = new SelectList(_context.Topics, "Id", "Id");
            ViewBag.Topics = await _context.Topics.ToListAsync();

            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Slug,Thumbnail,TopicID,Content")] BlogViewModel post)
        {
            //if(await _context.Blogs.AddAsync(p => p.Slug == post.Slug))
            //{
            //    ModelState.AddModelError("Slug", "Nhập Slug khác");
            //    return View(blog);
            //}

            if (ModelState.IsValid)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                post.CreateBy = Convert.ToInt32(user);
                post.CreatedAt = DateTime.Now;
               

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicID"] = new SelectList(_context.Topics, "Id", "Id", post.TopicID);





            return View(post);




            //using (var transaction = await _context.Database.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        Blog blog = new Blog()
            //        {
            //            //Thumbnail = blogModel.Thumbnail,
            //            Name = blogModel.Name,
            //            Slug = blogModel.Slug,
            //            CreatedAt = DateTime.Now,
            //            //CreateBy = user.Id,
            //            TopicID = Convert.ToInt32(blogModel.Topic),
            //            Content = blogModel.Content,
            //            IsDetele = false,
            //        };
            //        _context.Add(blog);
            //        await _context.SaveChangesAsync();
            //        transaction.Commit();

            //        return Ok(new { message = "Created blog successfully!" });
            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //        return BadRequest(new { message = "Failed to create blog." + ex.Message });
            //    }
            //}

        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["TopicID"] = new SelectList(_context.Topics, "Id", "Id", blog.TopicID);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Slug,Name,CreatedAt,CreateBy,TopicID,Content,IsDetele")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["TopicID"] = new SelectList(_context.Topics, "Id", "Id", blog.TopicID);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'AppDbContext.Blog'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
