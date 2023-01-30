using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheForumHubMVC.Data.Services;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminManager;
        private readonly IQuestionService _questionManager;

        public AdminController(IAdminService adminManager,
                               IQuestionService questionManager)
        {
            _adminManager = adminManager;
            _questionManager = questionManager;
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await _adminManager.GetQuestionReportsByIdAsync(id));
        }
        public async Task<IActionResult> Questions()
        {
            return View((await _adminManager.GetQuestionsReportAsync()));
        }
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _adminManager.RemoveReportAsync(id);
            if (result) return Redirect(Request.Headers["Referer"]);
            return RedirectToAction(nameof(Questions));
        }
        [HttpGet]
        public async Task<IActionResult> RemoveQuestion(int id)
        {
            await _adminManager.RemoveReportsAsync(id, Data.Enums.ReportType.QuestionType);
            return RedirectToAction(nameof(Questions));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _questionManager.GetQuestionByIdAsync(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adminManager.DeleteAsync(id, Data.Enums.ReportType.QuestionType);
            return RedirectToAction(nameof(Questions));
        }
    }
}
