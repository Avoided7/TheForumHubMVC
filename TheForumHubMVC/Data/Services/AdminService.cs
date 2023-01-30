using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Data.Enums;
using TheForumHubMVC.Data.ViewModels.Admin;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly ForumDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IQuestionService _questionManager;
        private readonly IAnswerService _answerManager;

        public AdminService(ForumDbContext context,
                            UserManager<User> userManager,
                            IQuestionService questionManager,
                            IAnswerService answerManager)
        {
            _context = context;
            _userManager = userManager;
            _questionManager = questionManager;
            _answerManager = answerManager;
        }
        public async Task<List<ReportAnswerVM>> GetAnswersReportAsync()
        {
            var reportsAnswer = _context.Reports.Where(r => r.ReportType == ReportType.AnswerType).ToList();
            var reportsVM = new List<ReportAnswerVM>();
            foreach (var report in reportsAnswer.DistinctBy(r => r.TypeId))
            {
                var answer = await _context.Answers.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == report.TypeId);
                reportsVM.Add(new ReportAnswerVM()
                {
                    Id = report.Id,
                    Answer = answer,
                    ReportCount = _context.Reports.Count(r => r.ReportType == ReportType.AnswerType && r.TypeId == answer.Id),
                    ReportContent = report.Content,
                    User = answer.User
                });
            }
            return reportsVM;
        }
        public async Task<IEnumerable<ReportAnswerVM>> GetAnswerReportsByIdAsync(int id)
        {
            var reportsAnswer = _context.Reports.Where(r => r.ReportType == ReportType.AnswerType && r.TypeId == id);
            var reportsVM = new List<ReportAnswerVM>();
            foreach (var report in reportsAnswer)
            {
                var answer = await _context.Answers.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == report.TypeId);
                reportsVM.Add(new ReportAnswerVM()
                {
                    Id = report.Id,
                    Answer = answer,
                    ReportCount = _context.Reports.Count(r => r.ReportType == ReportType.AnswerType && r.TypeId == answer.Id),
                    ReportContent = report.Content,
                    User = await _userManager.FindByIdAsync(report.UserId)
                });
            }
            return reportsVM;
        }
        public async Task<List<ReportQuestionVM>> GetQuestionReportsByIdAsync(int id)
        {
            var reports = _context.Reports.Where(r => r.ReportType == ReportType.QuestionType && r.TypeId == id);
            var reportsVM = new List<ReportQuestionVM>();
            foreach (var report in reports)
            {
                var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == report.TypeId);
                var user = await _userManager.FindByIdAsync(report.UserId);
                reportsVM.Add(new ReportQuestionVM()
                {
                    Id = report.Id,
                    ReportContent = report.Content,
                    Question = question,
                    User = user,
                    ReportCount = _context.Reports.Count(r => r.TypeId == report.TypeId && r.ReportType == report.ReportType)
                });
            }
            return reportsVM;
        }
        public async Task<List<ReportQuestionVM>> GetQuestionsReportAsync()
        {
            var reports = _context.Reports.Where(r => r.ReportType == Enums.ReportType.QuestionType).ToList();
            var reportsVM = new List<ReportQuestionVM>();
            foreach (var report in reports.DistinctBy(r => r.TypeId))
            {
                var question = await _context.Questions.Include(q => q.User).FirstOrDefaultAsync(q => q.Id == report.TypeId);
                var user = await _userManager.FindByIdAsync(report.UserId);
                reportsVM.Add(new ReportQuestionVM()
                {
                    Id = report.Id,
                    ReportContent = report.Content,
                    Question = question,
                    User = user,
                    ReportCount = _context.Reports.Count(r => r.TypeId == report.TypeId && r.ReportType == report.ReportType)
                });
            }
            return reportsVM;
        }
        public async Task<bool> RemoveReportAsync(int id)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
            if (report == null) return false;
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return _context.Reports.Any(r => r.TypeId == report.TypeId && r.ReportType == report.ReportType);
        }
        public async Task RemoveReportsAsync(int id, ReportType type)
        {
            var report = _context.Reports.Where(r => r.TypeId == id && r.ReportType == type);
            if (report == null) return;
            _context.Reports.RemoveRange(report);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id, ReportType type)
        {
            var reports = _context.Reports.Where(r => r.TypeId == id && r.ReportType == type);
            _context.Reports.RemoveRange(reports);
            switch (type)
            {
                case ReportType.QuestionType:
                    await _questionManager.DeleteQuestionAsync(id);
                    break;
                case ReportType.AnswerType:
                    await _answerManager.DeleteAnswerAsync(id);
                    break;
                default:
                    break;
            }

        }
    }
}
