using System.ComponentModel.DataAnnotations;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class ChangePasswordVM
    {
        [DataType(DataType.Password), Display(Name = "Current password"), MinLength(8)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password), Display(Name = "New password"), MinLength(8)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password), Display(Name = "Confirm new password"), Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
