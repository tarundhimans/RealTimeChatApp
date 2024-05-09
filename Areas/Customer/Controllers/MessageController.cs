using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RealTimeChatApp.Data;
using RealTimeChatApp.Models;
using System;
using System.Linq;


namespace RealTimeChatApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public MessageController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            if (file != null && file.Length > 0)
            {
                var message = new Message
                {
                    UserId = User.Identity.Name,
                    Text = model.Text,
                    Timestamp = DateTime.Now
                };

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                message.filepath = fileName;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
