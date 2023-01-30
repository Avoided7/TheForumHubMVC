using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class AnswerVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        public IUrlHelper UrlHelper { get; set; }
        public bool Notifications { get; set; } = false;
    }
}
