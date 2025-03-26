using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Assignment2.Pages.Staff
{
    [Authorize(Policy = "StaffOnly")]
    public class ProfileModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public ProfileModel(FunewsManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SystemAccount Account { get; set; }

        public IActionResult OnGet()
        {
            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Account = _context.SystemAccounts.Find(accountId);
            if (Account == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var existingAccount = await _context.SystemAccounts.FindAsync(accountId);
            if (existingAccount == null) return NotFound();

            existingAccount.AccountEmail = Account.AccountEmail;
            existingAccount.AccountName = Account.AccountName;
            existingAccount.AccountPassword = Account.AccountPassword;

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
