using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs
{  
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            //string username = "Issaka";
            Users.Add(Context.ConnectionId, username);
            await AddMessageToChat(string.Empty, $"{username} a rejoint le Chat!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{username} left!");
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
