using Microsoft.AspNetCore.Mvc;
using PrivsXYZ.MVC.Helpers;
using PrivsXYZ.MVC.Models;
using PrivsXYZ.MVC.Services;

namespace PrivsXYZ.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IClientInfoService _clientInfoService;

        public HomeController(IMessageService messageService, IClientInfoService clientInfoService)
        {
            _messageService = messageService;
            _clientInfoService = clientInfoService;
        }

        public async Task<IActionResult> Index()
        {
            var salt = RandomGeneratorHelper.GetRandomSalt(256);
            var enc = await _messageService.Encrypt("test test test", salt, "12345678901234567890123456789011");
            ViewBag.Test = enc;
            ViewBag.Dec = await _messageService.Decrypt(enc, salt, "12345678901234567890123456789011");
            return View();
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(MessageSendModel messageModel)
        {
            try
            {
                var userData = _clientInfoService.GetUserData();
                messageModel.SenderIPv4Address = userData.IPv4;
                messageModel.SenderHostname = userData.Hostname;

                var endOfLink = await _messageService.CreateAndEncryptMessage(messageModel);

                ViewBag.Link = $"https://privs.xyz/decrypt/{endOfLink}";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("IP")]
        public IActionResult Ip()
        {
            var userData = _clientInfoService.GetUserData();

            ViewBag.ipv4 = userData.IPv4;
            ViewBag.host = userData.Hostname;

            return View();
        }
    }
}