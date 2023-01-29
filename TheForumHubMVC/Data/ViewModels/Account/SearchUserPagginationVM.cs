namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class SearchUserPagginationVM
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
    }
}
