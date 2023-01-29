using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class DeleteVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(8), Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
