using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ShoeShop.ViewModels;
using ShoeShop.Data.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly UserManager<AppUser> userManager;

        //public BlogsController(UserManager<AppUser> userManager)
        //{
        //    this.userManager = userManager;
        //}
        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Blogs
                .Include(b => b.Topic)
                .Include(b => b.Thumbnail)
                .ToListAsync();
            return View(posts);
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
            ViewBag.Topics = await _context.Topics.ToListAsync();

            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModel post)
        {
            //if(await _context.Blogs.AddAsync(p => p.Slug == post.Slug))
            //{
            //    ModelState.AddModelError("Slug", "Nhập Slug khác");
            //    return View(blog);
            //}
            
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                post.CreateBy = user;
                post.CreatedAt = DateTime.Now;

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + post.Image.FileName;
                string filePath = Path.Combine("wwwroot/img/blogs", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    post.Image.CopyTo(fileStream);
                }

                Image img = new Image
                {
                    Name = uniqueFileName
                };


                Blog bl = new Blog
                {
                    Slug = post.Slug,
                    Name = post.Name,

                    Thumbnail = img,
                    CreateBy = user,
                    TopicID = post.TopicID,
                    Content = post.Content,
                    IsDetele = false
                    
                };

            Console.Write("*****&"+bl.Thumbnail.Name);


                _context.Add(bl);
                await _context.SaveChangesAsync();
                return Json(new {message = "Created post successful"});
            

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
