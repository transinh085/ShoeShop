using ShoeShop.Models;

namespace ShoeShop.Services
{
	public interface IPayPalService
	{
		Task<string> CreatePaymentUrl(Order model, HttpContext context);
		PaymentResponse PaymentExecute(IQueryCollection collections);
	}
}
