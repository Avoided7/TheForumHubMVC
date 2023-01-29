using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Data.ViewModels.Answer;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ForumDbContext _context;

        public AnswerService(ForumDbContext context)
        {
            _context = context;
        }
        public async Task<List<Answer>> GetAnswersAsync()
        {
            return await _context.Answers.Include(a => a.Question).Include(a => a.User).ToListAsync();
        }
        public async Task AddRatingAsync(AnswerRatingVM model)
        {
            var allAnswers = _context.AnswersRating.Where(rating => rating.AnswerId == model.AnswerId);
            if (allAnswers == null) return;
            var answer = await allAnswers.FirstOrDefaultAsync(r => r.UserId == model.UserId);
            if (answer != null)
            {
                _context.AnswersRating.Remove(answer);
                if (answer.Rating == model.Rating)
                {
                    await _context.SaveChangesAsync();
                    return;
                }
            }
            var rating = new AnswerRating()
            {
                AnswerId = model.AnswerId,
                UserId = model.UserId,
                Rating = model.Rating,
            };
            await _context.AnswersRating.AddAsync(rating);
            await _context.SaveChangesAsync();
        }
        public async Task<List<AnswerRating>> GetAnswersRatingAsync()
        {
            return await _context.AnswersRating.Include(ar => ar.User)
                                               .Include(ar => ar.Answer)
                                               .ToListAsync();     
        }
        public async Task<Answer?> GetAnswerByIdAsync(int id)
        {
            return (await _context.Answers.Include(a => a.User).Include(a => a.Question).FirstOrDefaultAsync(a => a.Id == id));
        }
        public async Task UpdateAnswerAsync(int id, AnswerVM model)
        {
            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == id);
            if (answer == null ) { return; }
            answer.Content = model.Content;
            answer.Modified = DateTime.Now;

            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAnswerAsync(int id)
        {
            var answer = _context.Answers.Include(a => a.Ratings).FirstOrDefault(a => a.Id == id);
            if (answer == null) return;
            var answersRating = answer.Ratings;

            _context.Answers.Remove(answer);
            _context.AnswersRating.RemoveRange(answersRating);
            
            await _context.SaveChangesAsync();
        }
    }
}
