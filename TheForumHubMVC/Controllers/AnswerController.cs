using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheForumHubMVC.Data.Services;
using TheForumHubMVC.Data.ViewModels.Admin;
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
        private readonly IAdminService _adminManager;

        public AnswerController(UserManager<User> userManager,
                                IAnswerService answerManager,
                                IAdminService adminManager)
        {
            _userManager = userManager;
            _answerManager = answerManager;
            _adminManager = adminManager;
        }
        [HttpGet]
        public async Task<IActionResult> Report(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Report(int id, ReportVM model)
        {
            if (!ModelState.IsValid) { ViewBag.Id = id; return View(model); }
            model.UserId = _userManager.GetUserId(User);
            model.TypeId = id;
            await _answerManager.ReportAsync(model);
            return RedirectToAction(actionName: "All", controllerName: "Questions");
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
            if (answer == null || (answer.UserId != user.Id && !User.IsInRole(Roles.Admin)))
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
            if (answer == null || (answer.UserId != user.Id && !User.IsInRole(Roles.Admin)))
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
            if (answer == null || (answer.UserId != _userManager.GetUserId(User) && !User.IsInRole(Roles.Admin)))
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
            if (userId != model.UserId || answer == null || answer.UserId != userId)
            {
                if (answer != null && User.IsInRole(Roles.Admin)) { }
                else
                {
                    ModelState.AddModelError("", "Error");
                    return View(model);
                }
            }
            await _adminManager.RemoveReportsAsync(answer.Id, Data.Enums.ReportType.AnswerType);
            await _answerManager.DeleteAnswerAsync(answer.Id);
            return RedirectToAction("Details", "Questions", new { id = model.QuestionId });
        }
        #endregion
    }
}
