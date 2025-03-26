using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Assignment2.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public LoginModel(FunewsManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.SystemAccounts
                .FirstOrDefault(a => a.AccountEmail == Email && a.AccountPassword == Password && a.Status == 1);

            if (user == null)
            {
                ErrorMessage = "Đăng nhập thất bại. Vui lòng kiểm tra email hoặc mật khẩu.";
                return Page();
            }

            // Chỉ cho phép Admin (3) và Staff (1) đăng nhập
            if (user.AccountRole != 1 && user.AccountRole != 3)
            {
                ErrorMessage = "Bạn không có quyền đăng nhập vào hệ thống.";
                return Page();
            }

            // Tạo claims cho người dùng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.AccountEmail),
                new Claim("AccountRole", user.AccountRole.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString())
            };

            if (user.AccountRole == 3)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false // Không lưu cookie lâu dài
            };

            // Đăng nhập người dùng
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToPage("/Index");
        }
    }
}
