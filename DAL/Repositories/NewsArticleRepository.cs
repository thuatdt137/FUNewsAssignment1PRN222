using DAL.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Assignment1MVC.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<NewsArticle> GetAllNewsArticles()
        {
            return _context.NewsArticles.Include(n => n.Category).ToList();
        }
        public IEnumerable<NewsArticle> GetAllActiveNewsArticles()
        {
            return _context.NewsArticles.Where(n => n.NewsStatus == true).Include(n => n.Category).Include(n => n.Tags).ToList();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return _context.NewsArticles.Include(n => n.Category).Include(n => n.Tags)
                .FirstOrDefault(a => a.NewsArticleId == id);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.NewsArticleId = Guid.NewGuid().ToString();
            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            _context.SaveChanges();
        }

        public void DeleteNewsArticle(string id)
        {
            var article = GetNewsArticleById(id);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                _context.SaveChanges();
            }
        }
    }
}
