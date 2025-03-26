using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Security.Claims;

namespace Assignment2.Pages.Staff
{
    [Authorize(Policy = "StaffOnly")]
    public class NewsManagementModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public NewsManagementModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> NewsArticles { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; }

        public void OnGet()
        {
            NewsArticles = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .ToList();
            Categories = _context.Categories.Where(c => c.IsActive == true).ToList();
            Tags = _context.Tags.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadData();
                return Page();
            }

            NewsArticle.CreatedById = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            NewsArticle.CreatedDate = DateTime.Now;
            _context.NewsArticles.Add(NewsArticle);
            await _context.SaveChangesAsync();

            if (SelectedTagIds != null && SelectedTagIds.Any())
            {
                foreach (var tagId in SelectedTagIds)
                {
                    _context.Database.ExecuteSqlRaw($"INSERT INTO NewsTag (NewsArticleId, TagId) VALUES ('{NewsArticle.NewsArticleId}', {tagId})");
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadData();
                return Page();
            }

            var existingArticle = await _context.NewsArticles
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == NewsArticle.NewsArticleId);
            if (existingArticle == null) return NotFound();

            existingArticle.NewsTitle = NewsArticle.NewsTitle;
            existingArticle.Headline = NewsArticle.Headline;
            existingArticle.NewsContent = NewsArticle.NewsContent;
            existingArticle.NewsSource = NewsArticle.NewsSource;
            existingArticle.CategoryId = NewsArticle.CategoryId;
            existingArticle.NewsStatus = NewsArticle.NewsStatus;

            existingArticle.Tags.Clear();
            if (SelectedTagIds != null && SelectedTagIds.Any())
            {
                var tags = await _context.Tags.Where(t => SelectedTagIds.Contains(t.TagId)).ToListAsync();
                existingArticle.Tags.AddRange(tags);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null) return NotFound();

            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        private void LoadData()
        {
            NewsArticles = _context.NewsArticles.ToList();
            Categories = _context.Categories.Where(c => c.IsActive == true).ToList();
            Tags = _context.Tags.ToList();
        }
    }
}
