﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoeShop.Data;
using ShoeShop.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ShoeShop.Controllers
{
	public class BlogController : Controller
	{
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
		public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
		{
            if (_context.Blogs != null)
            {
                var topicsWithCount = await _context.Topics
                   .Select(topic => new
                   {
                       Topic = topic,
                       blogCount = _context.Blogs.Count(blog => blog.TopicID == topic.Id)
                   })
                   .ToListAsync();
                ViewBag.topics = topicsWithCount;
                ViewBag.Blog = await _context.Blogs
                     .Include(blog => blog.Thumbnail)
                     .Include(blog => blog.User)
                     .OrderByDescending(blog => blog.CreatedAt)
                     .ToListAsync();
                return View();
            }
            return Problem("Entity set 'AppDbContext.Blog'  is null.");
        }

		public IActionResult Detail(int id)
		{
			return View();
		}


        [HttpGet, ActionName("allBlogs")]
        public IActionResult GetAllBlogs(
        int page = 1,
        int pageSize = 9,
        string query = "",
        string topics = ""
        )
        {
            var queryableBlogs = _context.Blogs
                .Where(b => !b.IsDetele)
                .Include(blog => blog.Topic)
                .Include(blog => blog.Thumbnail)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                queryableBlogs = queryableBlogs.Where(u =>
                    u.Slug.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    u.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                );
            }
            // Filter by topics
            if (!string.IsNullOrEmpty(topics))
            {
                string[] cate = topics.Split(',');
                queryableBlogs = queryableBlogs.Where(u => cate.Contains(u.TopicID.ToString()));
            }

           
            // Paginate results
            var totalItems = queryableBlogs.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var currentPageBlog = queryableBlogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                MaxDepth = 100,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            var result = new
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Result = currentPageBlog
            };

            return Ok(JsonSerializer.Serialize(result, options));
        }
    }

    
}
