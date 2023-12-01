using ShoeShop.Helpers;

namespace ShoeShop.Services
{
	public interface ISendMailService
	{
		Task SendMail(MailContent mailContent);

		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}
