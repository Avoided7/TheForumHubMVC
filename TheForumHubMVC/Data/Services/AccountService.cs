using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly ForumDbContext _context;

        public AccountService(ForumDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(string id)
        {
            var answerRatings = _context.AnswersRating.Where(ar => ar.UserId == id);
            var questionRatings = _context.QuestionsRating.Where(qr => qr.UserId == id);
            var questions = _context.Questions.Where(q => q.UserId == id);
            var answers = _context.Answers.Where(a => a.UserId == id);

            _context.AnswersRating.RemoveRange(answerRatings);
            _context.QuestionsRating.RemoveRange(questionRatings);

            foreach(var question in questions)
            {
                question.UserId = "406a3db5-6647-47b2-9356-ef15b72090aa";
            }
            foreach(var answer in answers)
            {
                answer.UserId = "406a3db5-6647-47b2-9356-ef15b72090aa";
            }
            _context.Questions.UpdateRange(questions);
            _context.Answers.UpdateRange(answers);
            await _context.SaveChangesAsync();
        }
    }
}
