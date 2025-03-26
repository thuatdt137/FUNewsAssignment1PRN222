using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assignment2.Pages.NewsArticles
{
	public class IndexModel : PageModel
	{
		private readonly INewsArticleService _newsArticleService;
		private readonly ICategoryService _categoryService;
		private readonly ITagService _tagService;
		private readonly FunewsManagementContext _context;

		public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, FunewsManagementContext funewsManagementContext)
		{
			_newsArticleService = newsArticleService;
			_categoryService = categoryService;
			_tagService = tagService;
			_context = funewsManagementContext;
		}
		public List<NewsArticle> newsArticles { get; set; }
		public List<SelectListItem> CategoryList { get; set; } = new();
		public List<SelectListItem> TagList { get; set; } = new();

		[BindProperty(SupportsGet = true)]
		public int? CategoryId { get; set; } // ID danh mục

		[BindProperty(SupportsGet = true)]
		public int? TagId { get; set; } // ID tag

		[BindProperty(SupportsGet = true)]
		public string SortOrder { get; set; } = "date_desc"; // Tiêu chí sắp xếp

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;

		public int TotalPages { get; set; }

		private const int PageSize = 5;
		public async Task<IActionResult> OnGetAsync()
		{
			if (int.Parse(User.FindFirst("AccountRole").Value) != 1) return Forbid();
			var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			// Lấy danh sách danh mục
			CategoryList = await _context.Categories
				.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
				.ToListAsync();

			// Lấy danh sách tag
			TagList = await _context.Tags
				.Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
				.ToListAsync();

			// Truy vấn bài viết
			IQueryable<NewsArticle> query = _context.NewsArticles
				.Include(n => n.Category)
				.Include(n => n.Tags);

			// Lọc theo danh mục
			if (CategoryId.HasValue)
			{
				query = query.Where(n => n.CategoryId == CategoryId);
			}

			// Lọc theo tag
			if (TagId.HasValue)
			{
				query = query.Where(n => n.Tags.Any(nt => nt.TagId == TagId));
			}

			// Sắp xếp bài viết
			query = SortOrder switch
			{
				"name_asc" => query.OrderBy(n => n.NewsTitle),
				"name_desc" => query.OrderByDescending(n => n.NewsTitle),
				"id_asc" => query.OrderBy(n => n.NewsArticleId),
				"id_desc" => query.OrderByDescending(n => n.NewsArticleId),
				_ => query.OrderByDescending(n => n.CreatedDate), // Mặc định theo ngày tạo mới nhất
			};

			// Tính tổng số trang
			int totalArticles = await query.CountAsync();
			TotalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);

			// Lấy bài viết theo trang
			newsArticles = await query
				.Skip((PageNumber - 1) * PageSize)
				.Take(PageSize)
				.ToListAsync();

			return Page();
		}
	}
}
