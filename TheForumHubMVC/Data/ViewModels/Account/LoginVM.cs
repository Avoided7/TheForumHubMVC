using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class LoginVM
    {
        [Required, MinLength(6)]
        public string Username { get; set; }
        [Required, MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
