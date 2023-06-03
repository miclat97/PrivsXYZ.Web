using Microsoft.AspNetCore.Mvc;
using PrivsXYZ.MVC.Helpers;
using PrivsXYZ.MVC.Models;
using PrivsXYZ.MVC.Services;
using System.Diagnostics;

namespace PrivsXYZ.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _messageService;

        public HomeController(ILogger<HomeController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var salt = RandomGeneratorHelper.GetRandomSalt(256);
            var enc = await _messageService.Encrypt("test test test", salt, "12345678901234567890123456789011");
            ViewBag.Test = enc;
            ViewBag.Dec = await _messageService.Decrypt(enc, salt, "12345678901234567890123456789011");
            return View();
        }

        public async Task<IActionResult> SendMessage()
        {
            return View();
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