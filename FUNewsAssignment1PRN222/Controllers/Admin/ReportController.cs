using Microsoft.AspNetCore.Mvc;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Xceed.Words.NET;
    using Business.Interfaces;
    using Xceed.Document.NET;

    public class ReportController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

		public IActionResult ExportToWord(DateTime startDate, DateTime endDate)
		{
			var articles = _newsArticleService.GetAllActiveNewsArticles()
				.Where(a => a.ModifiedDate >= startDate && a.ModifiedDate <= endDate)
				.Select(a => new
				{
					a.NewsTitle,
					Status = a.NewsStatus == true ? "Active" : "Inactive",
					ModifiedDate = a.ModifiedDate,
					Category = a.Category.CategoryName
				}).ToList();
			articles = articles.OrderBy(a => a.ModifiedDate).ToList();
			string filePath = Path.Combine(Path.GetTempPath(), "NewsArticleReport.docx");
			using (var doc = DocX.Create(filePath))
			{
				doc.InsertParagraph($"News Articles Report ({startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd})")
					.FontSize(16).Bold().SpacingAfter(20);

				var table = doc.AddTable(articles.Count + 1, 4);
				table.Design = TableDesign.ColorfulList;
				table.Rows[0].Cells[0].Paragraphs.First().Append("Title").Bold();
				table.Rows[0].Cells[1].Paragraphs.First().Append("Status").Bold();
				table.Rows[0].Cells[2].Paragraphs.First().Append("Modified Date").Bold();
				table.Rows[0].Cells[3].Paragraphs.First().Append("Category").Bold();

				for (int i = 0; i < articles.Count; i++)
				{
					table.Rows[i + 1].Cells[0].Paragraphs.First().Append(articles[i].NewsTitle);
					table.Rows[i + 1].Cells[1].Paragraphs.First().Append(articles[i].Status);
					table.Rows[i + 1].Cells[2].Paragraphs.First().Append(articles[i].ModifiedDate.ToString());
					table.Rows[i + 1].Cells[3].Paragraphs.First().Append(articles[i].Category);
				}

				doc.InsertTable(table);
				doc.Save();
			}

			byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "NewsArticleReport.docx");
		}

	}

}
