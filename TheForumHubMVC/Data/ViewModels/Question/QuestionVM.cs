using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class QuestionVM
    {
        [Required, MinLength(5)]
        public string Title { get; set; }
        [Required, MinLength(20)]
        public string Description { get; set; }
        [Required]
        public List<int> TagsId { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
    }
}
