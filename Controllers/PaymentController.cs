using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;
using PayPal.v1.Payments;
using ShoeShop.Services;
using ShoeShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using PayPal.v1.Invoices;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ShoeShop.Hubs;

namespace ShoeShop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayPalService _payPalService;
		private readonly AppDbContext _context;
		private readonly IHubContext<OrderHub> _orderHubContext;

		public PaymentController(AppDbContext context, IPayPalService payPalService, IHubContext<OrderHub> orderHubContext)
        {
			_context = context;
            _payPalService = payPalService;
			_orderHubContext = orderHubContext;
		}

		[HttpPost]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentViewModel paymentInfo)
        {
            var cartVariantSizeIds = paymentInfo.Cart.Select(ci => ci.VariantSizeId).ToList();
            var variantSize = await _context.VariantSizes
                .Where(v => cartVariantSizeIds.Contains(v.Id))
                .Select(v => new OrderDetail
                {
                    VariantSizeId = v.Id,
                    Price = v.Variant.Product.PriceSale != 0 ? v.Variant.Product.PriceSale : v.Variant.Product.Price,
                })
                .ToListAsync();
            foreach (var item in variantSize)
            {
                item.Quantity = paymentInfo.Cart.First(ci => ci.VariantSizeId == item.VariantSizeId).Quantity;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (paymentInfo.AddressId == -1)
            {
                paymentInfo.NewAddress.AppUserId = userId;
                _context.Add(paymentInfo.NewAddress);
                await _context.SaveChangesAsync();
                paymentInfo.AddressId = paymentInfo.NewAddress.Id;
            }

            var shippingMethod = await _context.ShippingMethods.FindAsync(paymentInfo.ShippingMethodId);

            var order = new Models.Order
            {
                AppUserId = userId,
                ShippingMethodId = paymentInfo.ShippingMethodId,
                PaymentMethod = paymentInfo.PaymentMethodId,
                SubTotal = variantSize.Sum(v => v.Quantity * v.Price),
                ShippingFee = shippingMethod.Cost,
                Description = paymentInfo.OrderDescription,
                OrderStatus = 0,
                Details = variantSize,
                AddressId = paymentInfo.AddressId,
            };

            _context.Add(order);
			await _context.SaveChangesAsync();
            await _orderHubContext.Clients.All.SendAsync("ReceiveOrderUpdate");

            if (paymentInfo.PaymentMethodId == 0)
            {
                TempData["OrderId"] = order.Id;
                return Json($"https://localhost:7107/Payment/PayPalReturn?payment_method=COD&success=1&order_id={order.Id}");
            }
            else
            {
                var url = await _payPalService.CreatePaymentUrl(order, HttpContext);
                return Json(url);
            }
        }

        [Route("checkout/success")]
        public IActionResult PaymentSuccess()
        {
            if (TempData["OrderId"] is int orderId)
            {
                var order = _context.Orders
                    .Where(o => o.Id == orderId)
                    .Select(o => new
                    {
                        o.Id,
                        ShippingMethod = o.ShippingMethod.Name,
                        PaymentMethod = o.PaymentMethod == 0 ? "Cash on delivery" : "Payment with Paypal",
                        o.SubTotal,
                        o.ShippingFee,
                        o.Description,
                        o.PaymentStatus,
                        o.OrderStatus,
                        o.Address,
                        Details = o.Details.Select(p => new
                        {
                            VariantSizeId = p.VariantSizeId,
                            ProductId = p.VariantSize.Variant.Product.Id,
                            ProductSlug = p.VariantSize.Variant.Product.Slug,
                            Name = p.VariantSize.Variant.Product.Name,
                            Thumbnail = p.VariantSize.Variant.Product.Thumbnail.Name,
                            Size = p.VariantSize.Size.Name,
                            Color = p.VariantSize.Variant.Color.Name,
                            p.Price,
                            p.Quantity,
                        }).ToList()
                    }).FirstOrDefault();
                if (order != null)
                {
                    ViewBag.Order = order;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> PayPalReturn(string payment_method, int success, int order_id)
        {
            if (success == 1)
            {
                if(payment_method == "PayPal")
                {
                    var order = _context.Orders.FirstOrDefault(o => o.Id == order_id);

                    if (order != null)
                    {
                        order.PaymentStatus = true;
                        await _context.SaveChangesAsync();
                    }
                }
                TempData["OrderId"] = order_id;
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return RedirectToAction("PaymentSuccess");
            }
        }
    }
}
