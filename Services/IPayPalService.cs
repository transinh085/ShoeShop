using ShoeShop.Models;

namespace ShoeShop.Services
{
	public interface IPayPalService
	{
		Task<string> CreatePaymentUrl(PaymentInformation model, HttpContext context);
		PaymentResponse PaymentExecute(IQueryCollection collections);
	}
}
