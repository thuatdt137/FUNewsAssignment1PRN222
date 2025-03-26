using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assignment2.Pages.Publics
{
    public class NewsDetailModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public NewsDetailModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public NewsArticle Article { get; set; }

        [BindProperty]
        public string CommentContent { get; set; }

        public IActionResult OnGet(string id)
        {
            Article = _context.NewsArticles
                .Where(n => n.NewsArticleId == id && n.NewsStatus == true)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Include(n => n.Comments).ThenInclude(c => c.Account)
                .FirstOrDefault();

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Login");
            }

            if (string.IsNullOrEmpty(CommentContent))
            {
                return RedirectToPage(new { id });
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var comment = new Comment
            {
                NewsArticleId = id,
                AccountId = accountId,
                Content = CommentContent,
                CreatedDate = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
