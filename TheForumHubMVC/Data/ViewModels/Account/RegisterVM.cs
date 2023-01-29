using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class RegisterVM
    {
        [Required, MinLength(6)]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
