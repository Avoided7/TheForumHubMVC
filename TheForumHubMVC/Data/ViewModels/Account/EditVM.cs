using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class EditVM
    {
        [Required, MinLength(6)]
        public string Username { get; set; }
        [Display(Name = "New logotype (optional)")]
        public IFormFile? NewLogoImg { get; set; }
        public bool Notifications { get; set; }
    }
}
