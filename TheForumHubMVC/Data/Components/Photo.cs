using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace TheForumHubMVC.Data.Components
{
    public class Photo : ViewComponent
    {
        public string Invoke(byte[] photo)
        {
            return "data:image;base64," + Convert.ToBase64String(photo);
        }
    }
}
