using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Data.Enum;
using System.Linq;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
	public class HomeController : Controller
	{
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
		{
			ViewData["pendingOrder"] = await _context.Orders.Where(o => o.OrderStatus == OrderStatus.Unconfirmed).CountAsync();
			ViewData["orderConfirm"] = await _context.Orders.Where(o => o.OrderStatus == OrderStatus.Confirmed).CountAsync();
			ViewData["orderToday"] = await _context.Orders.Where(o => o.CreatedAt.Value.Date == DateTime.Today).CountAsync();
			ViewData["earningToday"] = await _context.Orders.Where(o => o.CreatedAt.Value.Date == DateTime.Today).SumAsync(o => o.OrderStatus == OrderStatus.Confirmed ? o.SubTotal + o.ShippingFee : 0);
            ViewBag.LastestOrder = await _context.Orders
                    .Select(o => new
                    {
                        o.Id,
                        Total = o.SubTotal + o.ShippingFee,
                        o.OrderStatus,
                        o.CreatedAt,
                        Customer = o.AppUser.FullName,
                    })
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(10)
                    .ToListAsync();

            var topVariantSizeIds = _context.Orders
               .Where(o => o.OrderStatus == OrderStatus.Confirmed)
               .SelectMany(o => o.Details)
               .GroupBy(detail => detail.VariantSizeId)
               .OrderByDescending(group => group.Sum(detail => detail.Quantity))
               .Take(10)
               .Select(group => group.Key)
               .ToList();
            ViewBag.TopProduct = _context.VariantSizes.Where(s => topVariantSizeIds.Contains(s.Id))
                .Select(p => new
                {
                    p.Variant.Product.Id,
                    p.Variant.Product.Name,
                    p.Variant.Product.Slug,
                })
                .AsEnumerable()
                .DistinctBy(p => p.Id)
                .ToList();
            return View();
		}

        public IActionResult GetStatistic()
        {
            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = DateTime.Today.DayOfWeek;

            var currentWeekRevenue = _context.Orders
                .Where(o => o.CreatedAt.Value.Date >= today.AddDays(-(int)currentDayOfWeek))
                .GroupBy(o => o.CreatedAt.Value.Date.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key,
                    Earnings = g.Sum(o => o.OrderStatus == OrderStatus.Confirmed ? o.SubTotal + o.ShippingFee : 0)
                }).ToDictionary(item => item.DayOfWeek, item => item.Earnings);

            // Calculate revenue for the previous week
            var previousWeekRevenue = _context.Orders
                .Where(o => o.CreatedAt.Value.Date >= today.AddDays(-(int)currentDayOfWeek - 7) &&
                            o.CreatedAt.Value.Date < today.AddDays(-(int)currentDayOfWeek))
                .GroupBy(o => o.CreatedAt.Value.Date.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key,
                    Earnings = g.Sum(o => o.SubTotal + o.ShippingFee)
                })
                .ToDictionary(item => item.DayOfWeek, item => item.Earnings);

            var orderedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            var result = new
            {
                previousWeek = orderedDaysOfWeek.Select(dayOfWeek => previousWeekRevenue.ContainsKey(dayOfWeek) ? previousWeekRevenue[dayOfWeek] : 0).ToArray(),
                currentWeek = orderedDaysOfWeek.Select(dayOfWeek => currentWeekRevenue.ContainsKey(dayOfWeek) ? currentWeekRevenue[dayOfWeek] : 0).ToArray()
            };

            return Json(result);
        }
    }
}
