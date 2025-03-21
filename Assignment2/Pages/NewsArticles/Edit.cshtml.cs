using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment2.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public EditModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }
        [BindProperty]
        public NewsArticle? NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();
        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> TagList { get; set; } = new();


        public async Task<IActionResult> OnGetAsync(string? id)
        {
            NewsArticle = _newsArticleService.GetNewsArticleById(id);
            CategoryList = _categoryService.GetAllCategories()
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }).ToList();
            TagList = _tagService.GetAllTags()
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName }).ToList();
            SelectedTagIds = NewsArticle.Tags.Select(t =>  t.TagId).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewsArticle != null)
            {
                NewsArticle.Tags.Clear();
				foreach (var tagId in SelectedTagIds)
				{
					NewsArticle.Tags.Add(_tagService.GetTagById(tagId));
				}
				_newsArticleService.UpdateNewsArticle(NewsArticle);
				return RedirectToPage("./Index");

			}
			CategoryList = _categoryService.GetAllCategories()
			.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName }).ToList();
			TagList = _tagService.GetAllTags()
				.Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName }).ToList();
			SelectedTagIds = NewsArticle.Tags.Select(t => t.TagId).ToList();
			return Page();
        }
    }
}
