using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assignment2.Pages.Staff
{
    [Authorize(Policy = "StaffOnly")]
    public class NewsHistoryModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public NewsHistoryModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> NewsArticles { get; set; }

        public void OnGet()
        {
            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            NewsArticles = _context.NewsArticles
                .Where(n => n.CreatedById == accountId)
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
    }
}
