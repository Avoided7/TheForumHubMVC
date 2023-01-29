using TheForumHubMVC.Data.ViewModels.Mail;

namespace TheForumHubMVC.Data.Services
{
    public interface IEmailService
    {
        Task SendMail(MailVM model);
    }
}
