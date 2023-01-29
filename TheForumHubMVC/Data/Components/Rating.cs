using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using TheForumHubMVC.Data.ViewModels.Question;

namespace TheForumHubMVC.Data.Components
{
    public class Rating : ViewComponent
    {
        public IViewComponentResult Invoke(RatingToQuestionVM rating)
        {
            return View(rating);
        }
    }
}
