using Microsoft.AspNetCore.SignalR;

namespace ShoeShop.Hubs
{
	public class OrderHub : Hub
	{
		public async Task SendOrder()
		{
			await Clients.All.SendAsync("ReceiveOrder");
		}
	}
}
