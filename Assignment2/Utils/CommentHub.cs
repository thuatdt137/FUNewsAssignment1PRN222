using Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace Assignment2.Utils
{
    public class CommentHub : Hub
    {
        private readonly FunewsManagementContext _context;

        public CommentHub(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task SendComment(string newsArticleId, short accountId, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Content is null or empty, skipping save.");
                    return; // Không lưu nếu content rỗng
                }

                var comment = new Comment
                {
                    NewsArticleId = newsArticleId,
                    AccountId = accountId,
                    Content = content,
                    CreatedDate = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Comment saved: {content} for article {newsArticleId}");
                await Clients.All.SendAsync("ReceiveComment", newsArticleId, accountId, content, DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving comment: {ex.Message}");
                throw; // Ném lỗi để client nhận biết
            }
        }
    }
}
