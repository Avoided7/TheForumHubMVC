using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class QuestionVM
    {
        [Required, MinLength(5, ErrorMessage = "The Title length must be greater than 5 characters.")]
        public string Title { get; set; }
        [Required, MinLength(20, ErrorMessage = "The Description length must be greater than 20 characters.")]
        public string Description { get; set; }
        [Required, MaxLength(5, ErrorMessage = "The Maximum tags is 5."), Display(Name = "Tags")]
        public List<int> TagsId { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
    }
}
