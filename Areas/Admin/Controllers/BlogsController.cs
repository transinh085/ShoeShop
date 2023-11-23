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
using System.Collections;
using System.IO;
using Microsoft.AspNetCore.Http;
namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Blogs
                .Include(b => b.Topic)
                .Include(b => b.Thumbnail)
                .ToListAsync();
            List<AppUser> authors = await _userManager.Users.ToListAsync();

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
        public async Task<IActionResult> Create([FromForm] BlogViewModel post)
        {
            //if (await _context.Blogs.AddAsync(p => p.Slug == post.Slug))
            //{
            //    ModelState.AddModelError("Slug", "Nhập Slug khác");
            //    return View(blog);
            //}

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

            AppUser author = await _userManager.FindByIdAsync(user);
            Blog bl = new Blog
            {
                Slug = post.Slug,
                Name = post.Name,
                Thumbnail = img,
                User = author,
                TopicID = post.TopicId,
                Content = post.Content,
                IsDetele = false
            };

            _context.Add(bl);
            await _context.SaveChangesAsync();
            return Json(new { message = "Created post successful" });
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs .Include(b => b.Thumbnail)
                                            .Include(b => b.User)
                                            .FirstOrDefaultAsync(b => b.Id == id); ;
            //.FindAsync(id)
            if (blog == null)
            {
                return NotFound();
            }

                ViewData["TopicID"] = new SelectList(_context.Topics, "Id", "Name", blog.TopicID);
            BlogViewModel post = new BlogViewModel()
            {
                Name = blog.Name,
                Slug = blog.Slug,
                User = blog.User,
                CreatedAt = blog.CreatedAt,
                Content = blog.Content
            };
            return View(blog);
            //return RedirectToAction("Edit","Blogs");
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

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.IsDetele = true;
                _context.Update(blog);
                await _context.SaveChangesAsync();
            }
            return Ok(new { message = "Delete successfully" });
        }


        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
//    [HttpGet, ActionName("allBlogs")]
//    public IActionResult GetAllBlogs(
//int page = 1,
//    int pageSize = 9,
//    string query = "",
//    string topics = "",
//    string author = ""

//    )

//    {
//        var queryableBlogs = _context.Blogs
//            .Where(b => !b.IsDetele)
//            .Include(b => b.Topic)
//            .Include(b => b.Thumbnail)
//            .AsQueryable();
//    }


