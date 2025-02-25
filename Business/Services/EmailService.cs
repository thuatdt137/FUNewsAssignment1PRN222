using Business.Interfaces;
using Domain.DTO;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailRequest request)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            string senderEmail = emailSettings["SenderEmail"];
            string senderPassword = emailSettings["SenderPassword"];
            string smtpServer = emailSettings["SmtpServer"];
            int smtpPort = int.Parse(emailSettings["SmtpPort"]);

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(senderEmail, senderPassword);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("FUNews Management System", senderEmail));
            email.To.Add(new MailboxAddress("", request.To));
            email.Subject = request.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = request.Body };
            email.Body = bodyBuilder.ToMessageBody();

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            Console.WriteLine($"Email sent successfully to {request.To}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
    }
}
