using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;
        public AuthController (ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _systemAccountService.Authenticate(email, password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }

            // Lưu thông tin user vào session
            HttpContext.Session.SetString("UserId", user.AccountId.ToString());
            HttpContext.Session.SetString("UserName", user.AccountEmail);
            HttpContext.Session.SetInt32("UserRole", user.AccountRole.GetValueOrDefault());

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Login", "Auth"); // Chuyển hướng về trang Login
        }

    }
}
