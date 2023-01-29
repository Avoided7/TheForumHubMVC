using TheForumHubMVC.Models;
namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class FilterVM
    {
        public string Categorie { get; set; }
        public Tag Tag { get; set; }
        public IEnumerable<Models.Question> Questions { get; set; }
    }
}
