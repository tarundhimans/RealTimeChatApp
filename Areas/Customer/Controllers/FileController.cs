using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace RealTimeChatApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendFile(IFormFile file, string user, string message)
        {
            
            return Json(new { success = true, fileName = file.FileName });
        }
    }
}

