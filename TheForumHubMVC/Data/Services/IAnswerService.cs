using TheForumHubMVC.Data.ViewModels.Admin;
using TheForumHubMVC.Data.ViewModels.Answer;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public interface IAnswerService
    {
        Task AddRatingAsync(AnswerRatingVM model);
        Task DeleteAnswerAsync(int id);
        Task<Answer?> GetAnswerByIdAsync(int id);
        Task<List<Answer>> GetAnswersAsync();
        Task<List<AnswerRating>> GetAnswersRatingAsync();
        Task ReportAsync(ReportVM model);
        Task UpdateAnswerAsync(int id, AnswerVM model);
    }
}
