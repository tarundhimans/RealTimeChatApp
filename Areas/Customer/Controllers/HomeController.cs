using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Models;
using System.Diagnostics;

namespace RealTimeChatApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {   
            if (!User.Identity.IsAuthenticated)
            {
               
                return Redirect("/Identity/Account/Login");
            }           
            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
