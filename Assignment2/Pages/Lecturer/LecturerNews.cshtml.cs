using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Pages.Lecturer
{
	[Authorize]
	public class LecturerNewsModel : PageModel
	{
		private readonly FunewsManagementContext _context;

		public LecturerNewsModel(FunewsManagementContext context)
		{
			_context = context;
		}

		public List<NewsArticle> NewsArticles { get; set; }

		public IActionResult OnGet()
		{
			var role = int.Parse(User.FindFirst("AccountRole").Value);
			if (role != 2) return Forbid();

			NewsArticles = _context.NewsArticles
				.Where(n => n.NewsStatus == true)
				.Include(n => n.Category)
				.Include(n => n.Tags)
				.ToList();
			return Page();
		}
	}
}
