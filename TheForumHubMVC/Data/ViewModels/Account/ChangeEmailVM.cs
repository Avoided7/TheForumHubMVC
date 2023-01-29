using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class ChangeEmailVM
    {
        [Display(Name = "New email"), EmailAddress]
        public string NewEmail { get; set; }
        [DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; }
    }
}
