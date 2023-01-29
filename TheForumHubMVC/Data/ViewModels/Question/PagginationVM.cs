namespace TheForumHubMVC.Data.ViewModels.Question
{
    public class PagginationVM
    {
        public string Action { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string Categorie { get; set; } = "";
        public string Tag { get; set; } = "";
    }
}
