using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using ShoeShop.Data.Enum;
using ShoeShop.Hubs;

namespace ShoeShop.Areas.Admin.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	[Area("Admin")]
    public class OrdersController : Controller
    {
        public readonly AppDbContext _context;
        IHubContext<OrderHub> _orderHubContext;


        public OrdersController(AppDbContext context, IHubContext<OrderHub> orderHubContext)
        {
            _context = context;
            _orderHubContext = orderHubContext;

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

        public async Task<IActionResult> GetOrders(string query, string dateStart, string dateEnd, int status)
        {
            try
            {
                var draw = int.Parse(Request.Form["draw"].FirstOrDefault());
                var skip = int.Parse(Request.Form["start"].FirstOrDefault());
                var pageSize = int.Parse(Request.Form["length"].FirstOrDefault());
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var orderData = _context.Orders
                    .Select(o => new
                    {
                        o.Id,
                        PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                        ShippingMethod = o.ShippingMethod.Name,
                        Total = o.SubTotal + o.ShippingFee,
                        o.OrderStatus,
                        o.CreatedAt,
                        Customer = o.AppUser.FullName,
                    })
                    .OrderByDescending(o => o.Id)
                    .AsQueryable();

                switch (sortColumn.ToLower())
                {
                    case "id":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.Id) : orderData.OrderByDescending(o => o.Id);
                        break;
                    case "total":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.Total) : orderData.OrderByDescending(o => o.Total);
                        break;
                    case "submited":
                        orderData = sortColumnDirection.ToLower() == "asc" ? orderData.OrderBy(o => o.CreatedAt) : orderData.OrderByDescending(o => o.CreatedAt);
                        break;
                    default:
                        orderData = orderData.OrderBy(o => o.Id);
                        break;
                }

                if (!string.IsNullOrEmpty(query))
                {
                    orderData = orderData.Where(o => o.Id.ToString().Contains(query));
                }

                if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateEnd))
                {
                    var startDate = DateTime.Parse(dateStart);
                    var endDate = DateTime.Parse(dateEnd).AddDays(1);
                    orderData = orderData.Where(o => o.CreatedAt >= startDate && o.CreatedAt < endDate);
                }

                if (status != -2)
                {
                    var orderStatusFilter = (OrderStatus)status;
                    orderData = orderData.Where(o => o.OrderStatus == orderStatusFilter);
                }

                var recordsTotal = await orderData.CountAsync();
                var data = await orderData.Skip(skip).Take(pageSize).ToListAsync();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
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
                        o.CreatedAt,
                        Customer = o.AppUser,
                        Details = o.Details.Select(p => new
                        {
                            VariantSizeId = p.VariantSizeId,
                            ProductSlug = p.VariantSize.Variant.Product.Slug,
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
            var order = await _context.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.VariantSize)
                .FirstOrDefaultAsync(o => o.Id == id);
            if(order != null)
            {
                order.OrderStatus = OrderStatus.Confirmed;

                order.Details.ForEach(d =>
                {
                    d.VariantSize.Quantity = d.VariantSize.Quantity - d.Quantity;
                });

                await _context.SaveChangesAsync();

                await _orderHubContext.Clients.All.SendAsync("ReceiveOrderUpdate");
                return Json(new { status = "Confirmed" });
            }
            return Json(new { status = "Not found order id" });
        }
    }
}
