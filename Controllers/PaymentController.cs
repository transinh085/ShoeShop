using Microsoft.AspNetCore.Mvc;
using ShoeShop.Models;
using System.Net;
using PayPal.Core;
using PayPal.v1.Payments;
using ShoeShop.Services;
using ShoeShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Data;
using PayPal.v1.Invoices;

namespace ShoeShop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayPalService _payPalService;
		private readonly AppDbContext _context;

		public PaymentController(AppDbContext context, IPayPalService payPalService)
        {
			_context = context;
            _payPalService = payPalService;
        }

		[HttpPost]
		public async Task<IActionResult> CreatePaymentUrl([FromBody]PaymentViewModel paymentInfo)
        {
			var cartVariantSizeIds = paymentInfo.Cart.Select(ci => ci.VariantSizeId).ToList();
			var variantSize = await _context.VariantSizes
					.Where(v => cartVariantSizeIds.Contains(v.Id))
					.Select(v => new PaymentItem
					{
						ProductId = v.Variant.Product.Id,
						Title = v.Variant.Product.Name + " - Size " + v.Size.Name + " - Color " + v.Variant.Color.Name,
						VariantSizeId = v.Id,
						Price = v.Variant.Product.PriceSale != 0 ? v.Variant.Product.PriceSale : v.Variant.Product.Price,
					})
					.ToListAsync();

			foreach (var item in variantSize)
            {
                item.Quantity = paymentInfo.Cart.First(ci => ci.VariantSizeId == item.VariantSizeId).Quantity;
			}

			PaymentInformation model = new PaymentInformation
			{
				Items = variantSize,
				Amount = variantSize.Sum(v => v.Quantity * v.Price),
				ShippingCost = _context.ShippingMethods.First(p => p.Id  == paymentInfo.ShippingMethodId).Cost,
				Description = paymentInfo.OrderDescription
			};

			var url = await _payPalService.CreatePaymentUrl(model, HttpContext);

			return Json(url);
		}

        public IActionResult PaymentCallback()
        {
            var response = _payPalService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
