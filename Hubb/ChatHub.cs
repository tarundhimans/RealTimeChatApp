using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace RealTimeChatApp.Hubb
{
    public class ChatHub : Hub
    {
        static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            users.TryAdd(Context.ConnectionId, username);

            // Send the UserConnected event to all other clients
            await Clients.Others.SendAsync("UserConnected", username);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            users.TryRemove(Context.ConnectionId, out var username);

            await Clients.All.SendAsync("UserDisconnected", username);
            await base.OnDisconnectedAsync(exception);
        }

        public IEnumerable<string> GetOnlineUsers()
        {
            return users.Values.Distinct();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendFile(IFormFile file, string user, string message)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            await Clients.All.SendAsync("ReceiveFile", user, fileName);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
