using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class AnswerVM
    {
        [ValidateNever]
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        public IUrlHelper UrlHelper { get; set; }
        public bool Notifications { get; set; } = false;
    }
}
