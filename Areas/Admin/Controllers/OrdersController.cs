using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        public readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.Select(o => new
            {
                o.Id,
                PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                ShippingMethod = o.ShippingMethod.Name,
                Total = o.SubTotal + o.ShippingFee,
                o.PaymentStatus,
                o.OrderStatus,
                o.CreatedAt,
                Customer = o.AppUser,
            }).ToList();
            ViewBag.Orders = orders;
            return View();
        }

        public IActionResult Detail(int id)
        {
            var order = _context.Orders
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                        ShippingMethod = o.ShippingMethod.Name,
                        o.SubTotal,
                        o.ShippingFee,
                        o.Description,
                        o.PaymentStatus,
                        o.OrderStatus,
                        o.Address,
                        Customer = o.AppUser,
                        Details = o.Details.Select(p => new
                        {
                            VariantSizeId = p.VariantSizeId,
                            ProductId = p.VariantSize.Variant.Product.Id,
                            Name = p.VariantSize.Variant.Product.Name,
                            Size = p.VariantSize.Size.Name,
                            Color = p.VariantSize.Variant.Color.Name,
                            Stock = p.VariantSize.Quantity,
                            p.Price,
                            p.Quantity,
                        }).ToList()
                    }).FirstOrDefault();
            if(order == null)
            {
                return NotFound();
            }
            ViewBag.Order = order;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order != null)
            {
                order.OrderStatus = 1;
                _context.SaveChangesAsync();
                return Json(new { status = "Confirmed" });
            }
            return Json(new { status = "Not found order id" });
        }
    }
}
