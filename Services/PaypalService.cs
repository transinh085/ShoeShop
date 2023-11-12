using PayPal.Core;
using PayPal.v1.Payments;
using ShoeShop.Models;
using System.Net;

namespace ShoeShop.Services
{
    public class PaypalService : IPayPalService
	{
        private readonly IConfiguration _configuration;

        public PaypalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Item> convertList(List<PaymentItem> items)
        {
            return items.Select(v => new Item
            {
				Name = v.Title,
				Currency = "USD",
				Price = v.Price.ToString(),
				Quantity = v.Quantity.ToString(),
				Sku = "sku",
				Tax = "0",
				Url = $"https://localhost:7107/Product/Detail/{v.ProductId}",
			}).ToList();

		}

		public async Task<string> CreatePaymentUrl(PaymentInformation model, HttpContext context)
        {
            var envSandbox =
                new SandboxEnvironment(_configuration["Paypal:ClientId"], _configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envSandbox);
            var paypalOrderId = DateTime.Now.Ticks;
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
                            Total = (model.Amount + model.ShippingCost).ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = model.ShippingCost.ToString(),
                                Subtotal = model.Amount.ToString(),
                            }
                        },
                        ItemList = new ItemList()
                        {
                            Items = convertList(model.Items)
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
