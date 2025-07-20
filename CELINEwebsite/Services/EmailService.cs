using MailKit.Net.Smtp;
using MimeKit;
using CELINEwebsite.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace CELINEwebsite.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings,
                          ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string name, string email, string message)
        {
            try
            {
                _logger.LogInformation("Preparing email message...");
                var emailMessage = new MimeMessage();

                // إعداد المرسل (استخدام بريد الموقع الرسمي) والمستقبل
                emailMessage.From.Add(new MailboxAddress("Website Contact Form", _emailSettings.FromEmail));
                emailMessage.To.Add(new MailboxAddress("Admin", _emailSettings.ToEmail));
                emailMessage.Subject = $"New Contact from {name} ({email})"; // إضافة البريد في العنوان

                // نص الرسالة
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Name: {name}\nEmail: {email}\n\nMessage:\n{message}"
                };

                _logger.LogInformation("Connecting to SMTP server...");
                using var client = new SmtpClient();

                // هذه السطور مهمة للتغلب على مشاكل SSL
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    MailKit.Security.SecureSocketOptions.StartTls);

                _logger.LogInformation("Authenticating...");
                await client.AuthenticateAsync(
                    _emailSettings.SmtpUsername,
                    _emailSettings.SmtpPassword);

                _logger.LogInformation("Sending email...");
                await client.SendAsync(emailMessage);
                _logger.LogInformation("Email sent successfully.");

                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
                throw;
            }
        }
    }
}