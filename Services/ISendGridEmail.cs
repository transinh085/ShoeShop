namespace ShoeShop.Services
{
	public interface ISendGridEmail
	{
		Task SendEmailAsync(string toEmail, string subject, string message);
	}
}
