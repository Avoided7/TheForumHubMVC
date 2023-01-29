using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TheForumHubMVC.Data.ViewModels.Mail;

namespace TheForumHubMVC.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMail(MailVM model)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_configuration.GetSection("Mail:From").Value));
            message.To.Add(MailboxAddress.Parse(model.Email));
            message.Subject = model.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = model.Body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration.GetSection("Mail:Username").Value,
                                   _configuration.GetSection("Mail:Password").Value);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
