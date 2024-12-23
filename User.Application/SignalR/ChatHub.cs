using Microsoft.AspNetCore.SignalR;

namespace User.Application.SignalR
{
    public class ChatHub : Hub
    {
        // This method can be called by the client to send a message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

}
