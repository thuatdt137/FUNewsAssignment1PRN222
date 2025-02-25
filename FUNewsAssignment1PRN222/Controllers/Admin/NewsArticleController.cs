using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsAssignment1PRN222.Controllers.Admin
{
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IConfiguration _configuration;
        public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IConfiguration configuration)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _configuration = configuration;
        }
        // GET: NewsArticleController
        public ActionResult Index()
        {
            var articles = _newsArticleService.GetAllNewsArticles();
            return View("~/Views/Admin/NewsArticle/Index.cshtml", articles);
        }

        // GET: NewsArticleController/Details/5
        public ActionResult Details(string id)
        {
            var article = _newsArticleService.GetNewsArticleById(id);
            return View("~/Views/Admin/NewsArticle/Details.cshtml", article);
        }

        // GET: NewsArticleController/Create
        public ActionResult Create()
        {
            var categories = _categoryService.GetAllCategories();
            var tags = _tagService.GetAllTags();
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Không có danh mục nào.");
            }
            ViewBag.Tags = new SelectList(tags, "TagId", "TagName");
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View("~/Views/Admin/NewsArticle/Create.cshtml");
        }

        // POST: NewsArticleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsArticle newsArticle, List<int> Tags)
        {

            //if (ModelState.IsValid)
            //{
            foreach (var tagId in Tags)
            {
                newsArticle.Tags.Add(_tagService.GetTagById(tagId));
            }
            _newsArticleService.CreateNewsArticle(newsArticle);
            var authorName = HttpContext.Session.GetString("UserName") ?? "Unknown";
            var recipientEmail = _configuration["EmailSettings:AdminEmail"]; // Lấy email từ cấu hình

            _newsArticleService.NotifyUserAsync(recipientEmail, newsArticle.NewsTitle, authorName, newsArticle.NewsArticleId);

            return RedirectToAction(nameof(Index));
            //}
            var categories = _categoryService.GetAllCategories();
            var tags = _tagService.GetAllTags();

            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Không có danh mục nào.");
            }
            ViewBag.Tags = new SelectList(tags, "TagId", "TagName");

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View("~/Views/Admin/NewsArticle/Create.cshtml", newsArticle);
        }

        // GET: NewsArticleController/Edit/5
        public ActionResult Edit(string id)
        {
            var article = _newsArticleService.GetNewsArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Active" },
                new SelectListItem { Value = "false", Text = "Inactive" }
            };

            var categories = _categoryService.GetAllCategories();
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Không có danh mục nào.");
            }
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", article.CategoryId);
            ViewBag.SelectedTags = article.Tags.Select(t => t.TagId).ToList();
            ViewBag.Tags = new SelectList(_tagService.GetAllTags(), "TagId", "TagName");
            return View("~/Views/Admin/NewsArticle/Edit.cshtml", article);
        }

        // POST: NewsArticleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, NewsArticle newsArticle, List<int> Tags)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {
                newsArticle.Tags = new List<Tag>();
                foreach (var tagId in Tags)
                {
                    newsArticle.Tags.Add(_tagService.GetTagById(tagId));
                }
                _newsArticleService.UpdateNewsArticle(newsArticle);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAllCategories(), "CategoryId", "CategoryName", newsArticle.CategoryId);
            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Active" },
                new SelectListItem { Value = "false", Text = "Inactive" }
            };

            return View(newsArticle);
        }

        // GET: NewsArticleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsArticleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
