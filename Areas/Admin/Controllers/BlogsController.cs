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
using ShoeShop.Helpers;

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
                .Include(b => b.User)
                .ToListAsync();
            ViewBag.Topics = await _context.Topics.ToListAsync();
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
                Id = blog.Id,
                Thumbnail = blog.Thumbnail,
                Name = blog.Name,
                Slug = blog.Slug,
                User = blog.User,
                CreatedAt = blog.CreatedAt,
                TopicId = blog.TopicID,
                Content = blog.Content
            };
            return View(post);
            //return RedirectToAction("Edit","Blogs");
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,Content,Image,TopicID")] BlogViewModel bl)
        //public async Task<IActionResult> Edit(int id, [FromForm]Blog blog) 
        {
            if (id != bl.Id)
            {
                return NotFound();
            }
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + bl.Image.FileName;
            string filePath = Path.Combine("wwwroot/img/blogs", uniqueFileName);

            var img = new Image();
            if (bl.Image.FileName != "")
            {
                img = new Image
                {
                    Name = uniqueFileName
                };
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    bl.Image.CopyTo(fileStream);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var blog = await _context.Blogs.FindAsync(id);

                    blog.Name = bl.Name;
                    blog.Slug = bl.Slug;
                    blog.Content = bl.Content;
                    if (img != null)
                    {
                        blog.Thumbnail = img;
                    }
                    blog.TopicID = bl.TopicId;

                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                    return RedirectToAction(nameof(Index)); // Redirect to the list of blogs after successful edit
                }

                return View(bl);
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

        [HttpPost]
        public async Task<IActionResult> GetBlogs(string query, int[] topics)
        {
            var draw = int.Parse(Request.Form["draw"].FirstOrDefault());
            var skip = int.Parse(Request.Form["start"].FirstOrDefault());
            var pageSize = int.Parse(Request.Form["length"].FirstOrDefault());
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            var Blogs = _context.Blogs
                .Include(b => b.Thumbnail)
                .Include(b => b.User)
                .Include(b => b.Topic)
                .OrderByDescending(b => b.CreatedAt)
                .Where(p => !p.IsDetele)
                .AsQueryable();

            switch (sortColumn.ToLower())
            {
                case "id":
                    Blogs = sortColumnDirection.ToLower() == "asc" ? Blogs.OrderBy(o => o.Id) : Blogs.OrderByDescending(o => o.Id);
                    break;
                case "name":
                    Blogs = sortColumnDirection.ToLower() == "asc" ? Blogs.OrderBy(o => o.Name) : Blogs.OrderByDescending(o => o.Name);
                    break;
                case "topic":
                    Blogs = sortColumnDirection.ToLower() == "asc" ? Blogs.OrderBy(o => o.Topic.Name) : Blogs.OrderByDescending(o => o.Topic.Name);
                    break;
                case "user":
                    Blogs = sortColumnDirection.ToLower() == "asc" ? Blogs.OrderBy(o => o.User.FullName) : Blogs.OrderByDescending(o => o.User.FullName);
                    break;
                default:
                    Blogs = Blogs.OrderBy(o => o.Id);
                    break;
            }

            if (!string.IsNullOrEmpty(query))
            {
                Blogs = Blogs.Where(m => m.Name.Contains(query));
            }

            if (topics.Length != 0)
            {
                Blogs = Blogs.Where(u => topics.Contains(u.TopicID));
            }

            var recordsTotal = Blogs.Count();
            var data = Blogs.OrderByDescending(o => o.Id).Skip(skip).Take(pageSize).ToList();

            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }

    }
}



