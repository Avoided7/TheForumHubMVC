using TheForumHubMVC.Data.Enums;
using TheForumHubMVC.Data.ViewModels.Admin;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public interface IAdminService
    {
        Task DeleteAsync(int id, ReportType type);
        Task<List<ReportQuestionVM>> GetQuestionsReportAsync();
        Task<List<ReportQuestionVM>> GetQuestionReportsByIdAsync(int id);
        Task<bool> RemoveReportAsync(int id);
        Task RemoveReportsAsync(int id, ReportType type);
        Task<List<ReportAnswerVM>> GetAnswersReportAsync();
        Task<IEnumerable<ReportAnswerVM>> GetAnswerReportsByIdAsync(int id);
    }
}
