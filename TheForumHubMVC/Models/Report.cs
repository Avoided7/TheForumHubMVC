using TheForumHubMVC.Data.Enums;

namespace TheForumHubMVC.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public string UserId { get; set; } = "";
        public ReportType ReportType { get; set; }
        public int TypeId { get; set; }
    }
}
