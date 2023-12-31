using Microsoft.AspNetCore.SignalR;

namespace LMS.Hubs
{
    public class Notification : Hub
    {
        public async Task sendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}