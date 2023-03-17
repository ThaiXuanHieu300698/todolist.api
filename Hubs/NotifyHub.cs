using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TodoList.Api.Hubs
{
    public class NotifyHub : Hub
    {
        public async Task PushNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotify", message);
        }
    }
}