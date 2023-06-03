using Microsoft.AspNetCore.Mvc;

namespace PrivsXYZ.Web.Controllers
{
    public class PrivsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage()
        {
            return View();
        }
    }
}
