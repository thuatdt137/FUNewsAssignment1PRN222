using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public ReportModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> NewsArticles { get; set; }
        public Dictionary<string, int> ReportData { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public string ReportType { get; set; } // "Day", "Week", "Month"

        public void OnGet()
        {
            // Mặc định hiển thị thống kê 30 ngày gần nhất theo ngày
            EndDate = DateTime.Today;
            StartDate = EndDate.AddDays(-30);
            ReportType = "Day";
            GenerateReport();
        }

        public IActionResult OnPost()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
                return Page();
            }

            GenerateReport();
            return Page();
        }

        private void GenerateReport()
        {
            NewsArticles = _context.NewsArticles
                .Where(n => n.CreatedDate >= StartDate && n.CreatedDate <= EndDate)
                .ToList();

            ReportData = new Dictionary<string, int>();

            switch (ReportType)
            {
                case "Day":
                    var days = Enumerable.Range(0, (EndDate - StartDate).Days + 1)
                        .Select(d => StartDate.AddDays(d).ToString("dd/MM/yyyy"));
                    foreach (var day in days)
                    {
                        ReportData[day] = NewsArticles.Count(n => n.CreatedDate.ToString() == day);
                    }
                    break;

                case "Week":
                    var weeks = Enumerable.Range(0, (int)Math.Ceiling((EndDate - StartDate).TotalDays / 7) + 1)
                        .Select(w => StartDate.AddDays(w * 7).ToString("dd/MM/yyyy"));
                    foreach (var week in weeks)
                    {
                        var startOfWeek = DateTime.Parse(week).AddDays(-(int)DateTime.Parse(week).DayOfWeek);
                        var endOfWeek = startOfWeek.AddDays(6);
                        ReportData[$"Tuần {week}"] = NewsArticles.Count(n => n.CreatedDate >= startOfWeek && n.CreatedDate <= endOfWeek);
                    }
                    break;

                case "Month":
                    var months = Enumerable.Range(0, ((EndDate.Year - StartDate.Year) * 12) + EndDate.Month - StartDate.Month + 1)
                        .Select(m => StartDate.AddMonths(m).ToString("MM/yyyy"));
                    foreach (var month in months)
                    {
                        ReportData[month] = NewsArticles.Count(n => n.CreatedDate.ToString() == month);
                    }
                    break;
            }
        }
    }
}
