using Microsoft.AspNetCore.SignalR;

namespace AutoServiceProject.Hubs
{
    public class AppHub : Hub
    {
        public async Task SendToUser(string userId, string method, object message)
        {
            await Clients.User(userId).SendAsync(method, message);
        }

        public async Task Broadcast(string method, object message)
        {
            await Clients.All.SendAsync(method, message);
        }
    }
}
