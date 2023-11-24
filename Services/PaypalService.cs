using PayPal.Core;
using PayPal.v1.Payments;
using ShoeShop.Data;
using ShoeShop.Models;
using System.Net;

namespace ShoeShop.Services
{
    public class PaypalService : IPayPalService
	{
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public PaypalService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<Item> ConvertList(List<OrderDetail> items)
        {
            return items.Select(v =>
            {
                var p = _context.VariantSizes
                    .Where(p => p.Id == v.VariantSizeId)
                    .Select(p => new {
                        Id = p.Variant.Product.Id,
                        Slug = p.Variant.Product.Slug,
                        Name = p.Variant.Product.Name,
                        Size = p.Size.Name,
                        Color = p.Variant.Color.Name,
                     }).FirstOrDefault();

                return new Item
                {
                    Name = $"{p.Name} - Size {p.Size} - Color {p.Color}",
                    Currency = "USD",
                    Price = v.Price.ToString(),
                    Quantity = v.Quantity.ToString(),
                    Sku = "sku",
                    Tax = "0",
                    Url = $"https://localhost:7107/products/{p.Slug}",
                };

            }).ToList();
        }

        public async Task<string> CreatePaymentUrl(Models.Order model, HttpContext context)
        {
            var envSandbox =
                new SandboxEnvironment(_configuration["Paypal:ClientId"], _configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envSandbox);
            var paypalOrderId = model.Id;
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = (model.SubTotal + model.ShippingFee).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = model.ShippingFee.ToString(),
                                Subtotal = model.SubTotal.ToString(),
                            }
                        },
                        ItemList = new ItemList()
                        {
                            Items = ConvertList(model.Details)
						},
                        Description = $"Invoice #{model.Description}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    CancelUrl =
                        $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return paymentUrl;

            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }

            return paymentUrl;
        }

        public PaymentResponse PaymentExecute(IQueryCollection collections)
        {
            var response = new PaymentResponse();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_description"))
                {
                    response.OrderDescription = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("transaction_id"))
                {
                    response.TransactionId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_id"))
                {
                    response.OrderId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    response.PaymentMethod = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("success"))
                {
                    response.Success = Convert.ToInt32(value) > 0;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("paymentid"))
                {
                    response.PaymentId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payerid"))
                {
                    response.PayerId = value;
                }
            }

            return response;
        }
    }
}
