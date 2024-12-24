using Microsoft.AspNetCore.SignalR;
using OdevDagitimPortali.ViewModels;

namespace OdevDagitimPortali.Hubs
{
    public class GeneralHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}