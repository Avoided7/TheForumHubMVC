using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheForumHubMVC.Data.Services;
using TheForumHubMVC.Data.ViewModels.Answer;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAnswerService _answerManager;

        public AnswerController(UserManager<User> userManager,
                                IAnswerService answerManager)
        {
            _userManager = userManager;
            _answerManager = answerManager;
        }
        #region Rating
        public async Task<IActionResult> Add(int id)
        {
            await _answerManager.AddRatingAsync(new AnswerRatingVM
            {
                AnswerId = id,
                Rating = 1,
                UserId = _userManager.GetUserId(User)
            });
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Remove(int id)
        {
            await _answerManager.AddRatingAsync(new AnswerRatingVM
            {
                AnswerId = id,
                Rating = -1,
                UserId = _userManager.GetUserId(User)
            });
            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion
        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var answer = await _answerManager.GetAnswerByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (answer == null || (answer.UserId != user.Id && await _userManager.IsInRoleAsync(user, Roles.Admin)))
            {
                return NotFound();
            }

            return View(new AnswerVM()
            {
                Id = id,
                UserId = _userManager.GetUserId(User),
                QuestionId = answer.QuestionId ?? 0,
                Content = answer.Content
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AnswerVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.GetUserAsync(User);
            var answer = await _answerManager.GetAnswerByIdAsync(model.Id);
            if (answer != null && await _userManager.IsInRoleAsync(user, Roles.Admin))
            {

            }
            else if (answer == null || answer.UserId != user.Id || model.UserId != user.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return View(model);
            await _answerManager.UpdateAnswerAsync(model.Id, model);
            return RedirectToAction("Details", "Questions", new { id = model.QuestionId });
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            
            var answer = await _answerManager.GetAnswerByIdAsync(id);
            if (!User.IsInRole(Roles.Admin) && (answer == null || answer.UserId != _userManager.GetUserId(User)))
            {
                return NotFound();
            }

            return View(new AnswerVM()
            {
                Id = id,
                UserId = _userManager.GetUserId(User),
                QuestionId = answer.QuestionId ?? 0,
                Content = answer.Content
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(AnswerVM model)
        {
            var userId = _userManager.GetUserId(User);
            var answer = await _answerManager.GetAnswerByIdAsync(model.Id);
            if (!User.IsInRole(Roles.Admin) && (userId != model.UserId || answer == null || answer.UserId != userId))
            {
                ModelState.AddModelError("", "Error");
                return View(model);
            }
            await _answerManager.DeleteAnswerAsync(answer.Id);
            return RedirectToAction("Details", "Questions", new { id = model.QuestionId });
        }
        #endregion
    }
}
