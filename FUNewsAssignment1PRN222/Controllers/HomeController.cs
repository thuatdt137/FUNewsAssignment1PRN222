using Business.Interfaces;
using FUNewsAssignment1PRN222.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FUNewsAssignment1PRN222.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsArticleService _newsArticleService;

        public HomeController(ILogger<HomeController> logger, INewsArticleService newsArticleService)
        {
            _logger = logger;
            _newsArticleService = newsArticleService;
        }

        public IActionResult Index()
        {
            var articles = _newsArticleService.GetAllActiveNewsArticles();
            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
