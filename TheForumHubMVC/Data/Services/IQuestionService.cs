using TheForumHubMVC.Data.ViewModels;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public interface IQuestionService
    {
        Task<Question> GetQuestionByIdAsync(int id);
        Task<List<Question>> GetQuestionsAsync();
        Task<Question> CreateQuestionAsync(QuestionVM model);
        Task<Question> UpdateQuestionAsync(int id, QuestionVM model);
        Task DeleteQuestionAsync(int id);
        Task AddAnswerAsync(AnswerVM answer);
        Task AddRatingAsync(QuestionRatingVM model);
        Task<List<QuestionRating>> GetQuestionsRatingAsync();
        Task AddViewsAsync(ViewsVM model);
    }
}
