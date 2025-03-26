using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2.Pages.Auth
{
    public class GetAccountEmailModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public GetAccountEmailModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(short accountId)
        {
            var email = _context.SystemAccounts
                .Where(a => a.AccountId == accountId)
                .Select(a => a.AccountEmail)
                .FirstOrDefault();

            if (email == null)
            {
                return NotFound("Không tìm thấy tài khoản");
            }

            return Content(email);
        }
    }
}
