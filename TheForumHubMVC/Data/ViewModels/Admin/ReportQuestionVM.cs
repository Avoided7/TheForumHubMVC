using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.ViewModels.Admin
{
    public class ReportQuestionVM
    {
        public int Id { get; set; }
        public string ReportContent { get; set; }
        public int ReportCount { get; set; }
        public Models.Question Question { get; set; }
        public User User { get; set; }
    }
}
