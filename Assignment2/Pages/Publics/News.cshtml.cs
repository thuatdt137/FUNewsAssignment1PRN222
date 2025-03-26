using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assignment2.Pages.Publics
{
    public class NewsModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public NewsModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> NewsArticles { get; set; }

        public void OnGet()
        {
            NewsArticles = _context.NewsArticles
                .Where(n => n.NewsStatus == true) // Chỉ hiển thị bài active
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
    }
}