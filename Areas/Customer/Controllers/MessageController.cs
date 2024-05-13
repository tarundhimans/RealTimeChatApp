using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Data;
using RealTimeChatApp.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatApp.Hubb;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace RealTimeChatApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<MessageController> _logger;


        public MessageController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHubContext<ChatHub> hubContext, ILogger<MessageController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _hubContext = hubContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var allUsers = _context.Users.ToList();
            ViewBag.AllUsers = allUsers;

            // Fetch file paths
            var files = _context.Messages.Where(m => !string.IsNullOrEmpty(m.filepath)).ToList();
            ViewBag.Files = files;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(Message model, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Temporary error handling for debugging
                throw new Exception("No file received by the action.");
            }

            _logger.LogInformation("SendMessage called with file: " + file.FileName);

            if (file != null && file.Length > 0)
            {
                if (model.Text == null)
                {
                    model.Text = "hello1";
                }
                var message = new Message
                {
                    UserId = User.Identity.Name,
                    Text = model.Text,
                    Timestamp = DateTime.Now
                };

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                message.User = (ApplicationUser)user;

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                message.FileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                message.filepath = fileName;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                _logger.LogInformation("File saved with path: " + filePath);
                _logger.LogInformation("Message saved with ID: " + message.Id);

                // Get the SignalR hub context
                var hubContext = _hubContext.Clients.All;

                // Send a SignalR message to all clients
                await hubContext.SendAsync("ReceiveFile", User.Identity.Name, fileName);
                _logger.LogInformation("SignalR message sent with file name: " + fileName);
            }

            return RedirectToAction("Index");
        }

    }
}
