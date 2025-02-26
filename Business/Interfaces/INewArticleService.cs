using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface INewsArticleService
    {
        IEnumerable<NewsArticle> GetAllNewsArticles();
        IEnumerable<NewsArticle> GetAllActiveNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        bool DeleteNewsArticle(string id);
        Task NotifyUserAsync(string? recipientEmail, string articleTitle, string author, string? articleId);
        NewsArticle GetActiveNewsArticleById(string id);

    }
}
