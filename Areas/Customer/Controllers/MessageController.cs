using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Data;

namespace RealTimeChatApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
