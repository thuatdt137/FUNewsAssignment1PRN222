
using Business.Interfaces;
using DAL.Interfaces;
using Domain.DTO;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;

namespace Assignment1MVC.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public NewsArticleService(
            INewsArticleRepository newsArticleRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _newsArticleRepository = newsArticleRepository;
            _emailService = emailService;
            _configuration = configuration;
        }


        public IEnumerable<NewsArticle> GetAllNewsArticles()
        {
            return _newsArticleRepository.GetAllNewsArticles();
        }
        public IEnumerable<NewsArticle> GetAllActiveNewsArticles()
        {
            return _newsArticleRepository.GetAllActiveNewsArticles();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return _newsArticleRepository.GetNewsArticleById(id);
        }

        public IEnumerable <NewsArticle> GetActiveNewsArticles()
        {
            return GetAllActiveNewsArticles();
        }

        public void CreateNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = true;
            newsArticle.NewsArticleId = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
           .TrimEnd('=')
           .Substring(0, 20);
            _newsArticleRepository.AddNewsArticle(newsArticle);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            var existedNewsArticle = GetNewsArticleById(newsArticle.NewsArticleId);
            if (existedNewsArticle == null)
            {
                return;
            }

            existedNewsArticle.UpdatedById = newsArticle.UpdatedById;
            existedNewsArticle.ModifiedDate = DateTime.Now;

            // Cập nhật dữ liệu mới
            existedNewsArticle.NewsTitle = newsArticle.NewsTitle;
            existedNewsArticle.Headline = newsArticle.Headline;
            existedNewsArticle.NewsContent = newsArticle.NewsContent;
            existedNewsArticle.NewsSource = newsArticle.NewsSource;
            existedNewsArticle.CategoryId = newsArticle.CategoryId;
            existedNewsArticle.NewsStatus = newsArticle.NewsStatus;
            existedNewsArticle.Tags = newsArticle.Tags;

            _newsArticleRepository.UpdateNewsArticle(existedNewsArticle);
        }

        public void DeleteNewsArticle(string id)
        {
            _newsArticleRepository.DeleteNewsArticle(id);
        }
        public async Task NotifyUserAsync(string ?recipientEmail, string articleTitle, string author, string ?articleId)
        {
            string baseUrl = _configuration["AppSettings:BaseUrl"]; // Lấy BaseUrl từ appsettings.json
            string articleUrl = $"{baseUrl}/NewsArticle/Details/{articleId}";

            var emailRequest = new EmailRequest
            {
                To = recipientEmail,
                Subject = $"New Article Published: {articleTitle}",
                Body = $@"
            <h2>{articleTitle}</h2>
            <p><strong>Author:</strong> {author}</p>
            <p>Click <a href='{articleUrl}'>here</a> to read the full article.</p>"
            };

            await _emailService.SendEmailAsync(emailRequest);
        }




    }
}
