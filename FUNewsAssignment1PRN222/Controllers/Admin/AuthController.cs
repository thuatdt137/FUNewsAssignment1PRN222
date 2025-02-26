using Business.Interfaces;
using FUNewsAssignment1PRN222.Utils;
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
			if (user.Status == 0)
			{
				ViewBag.ErrorMessage = "Your Account are deactived.";
				return View();
			}


			// Lưu thông tin user vào session
			HttpContext.Session.SetString("UserId", user.AccountId.ToString());
            HttpContext.Session.SetString("UserName", user.AccountEmail);
            HttpContext.Session.SetString("UserRole", user.AccountRole.ToString());
			HttpContext.Session.SetString("UserStatus", user.Status.ToString());


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
