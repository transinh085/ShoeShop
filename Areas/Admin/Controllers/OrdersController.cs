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
            return View();
        }

        public IActionResult Detail(int id)
        {
            var order = _context.Orders
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        o.PaymentMethod,
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
            ViewBag.Order = order;
            return View();
        }
    }
}
