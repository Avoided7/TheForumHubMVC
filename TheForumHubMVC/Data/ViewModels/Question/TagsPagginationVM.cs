namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class TagsPagginationVM
    {
        public string Action { get; private set; } = "Tags";
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
    }
}
