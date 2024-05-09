using Microsoft.AspNetCore.SignalR;

namespace RealTimeChatApp.Hubb
{
    public class ChatHub : Hub
    {
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
