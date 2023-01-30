using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheForumHubMVC.Data.ViewModels.Admin
{
    public class ReportVM
    {
        public string Content { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]
        public int TypeId { get; set; }
    }
}
