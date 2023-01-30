using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.ViewModels.Admin
{
    public class ReportAnswerVM
    {
        public int Id { get; set; }
        public string ReportContent { get; set; }
        public int ReportCount { get; set; }
        public Models.Answer Answer { get; set; }
        public User User { get; set; }
    }
}
