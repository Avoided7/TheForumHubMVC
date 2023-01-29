using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Data.Helpers;
using TheForumHubMVC.Data.ViewModels;
using TheForumHubMVC.Data.ViewModels.Answer;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ForumDbContext _context;
        private readonly IEmailService _mailManager;

        public QuestionService(ForumDbContext context, IEmailService mailManager)
        {
            _context = context;
            _mailManager = mailManager;
        }

        public async Task AddViewsAsync(ViewsVM model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == model.QuestionId);
            if (question == null) return;

            // Views
            var views = model.Session.GetJson<List<int>>("Views");
            if (views == null)
            {
                views = new List<int>() { model.QuestionId };
                question.Views++;
            }
            if (!views.Contains(model.QuestionId)) { views.Add(model.QuestionId); question.Views++; }
            model.Session.SetJson("Views", views);

            
            _context.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task AddRatingAsync(QuestionRatingVM model)
        {
            var isAlreadyExists = await _context.QuestionsRating.FirstOrDefaultAsync(qr => qr.UserId == model.UserId && qr.QuestionId == model.QuestionId);
            if (isAlreadyExists != null || (await _context.Questions.FirstOrDefaultAsync(q => q.Id == model.QuestionId)) == null)
            {
                if (isAlreadyExists == null) return;
                _context.QuestionsRating.Remove(isAlreadyExists);
                if (isAlreadyExists.Rating == model.Rating)
                {
                    await _context.SaveChangesAsync();
                    return;
                }
            }
            await _context.QuestionsRating.AddAsync(new QuestionRating()
            {
                UserId = model.UserId,
                Rating = model.Rating,
                QuestionId = model.QuestionId
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddAnswerAsync(AnswerVM model)
        {
            var question = await GetQuestionByIdAsync(model.QuestionId);
            if (question == null) return;  
            var answer = new Answer()
            {
                Content = model.Content,
                QuestionId = model.QuestionId,
                UserId = model.UserId,
                SendedAt = DateTime.Now
            };
            if (model.Notifications && question.UserId != answer.UserId)
            {
                await _mailManager.SendMail(new ViewModels.Mail.MailVM()
                {
                    Email = question.User.Email,
                    Subject = "You recieved answer!",
                    Body = $"On your question \"{question.Title}\" got answer!<br /><a href=\"{model.UrlHelper.ActionLink(action: "Details", controller: "Questions", new { id = question.Id })}\">{model.UrlHelper.ActionLink(action: "Details", controller: "Questions", new { id = question.Id })}</a>"
                });
            }
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<Question> CreateQuestionAsync(QuestionVM model)
        {
            var question = new Question()
            {
                Title = model.Title,
                Description = model.Description,
                Asked = DateTime.Now,
                UserId = model.UserId
            };
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            foreach (var tag in _context.Tags.Where(t => model.TagsId.Contains(t.Id)))
            {
                await _context.QuestionTags.AddAsync(new Question_Tag()
                {
                    QuestionId = question.Id,
                    TagId = tag.Id
                });
            }

            await _context.SaveChangesAsync();
            return question;
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await GetQuestionByIdAsync(id);
            if (question == null) { throw new Exception("Not found question?!"); }

            var questionRating = question.Ratings;
            var answers = question.Answers;
            var questionTag = question.Question_Tags;

            foreach(var answer in answers)
            {
                var answerRating = answer.Ratings;
                _context.AnswersRating.RemoveRange(answerRating);
            }
            _context.QuestionTags.RemoveRange(questionTag);
            _context.Answers.RemoveRange(answers);
            _context.QuestionsRating.RemoveRange(questionRating);

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            var question = await _context.Questions
                                 .Include(q => q.User)
                                 .Include(q => q.Answers)
                                 .ThenInclude(a => a.Ratings)
                                 .Include(q => q.Answers)
                                 .ThenInclude(a => a.User)
                                 .Include(q => q.Ratings)
                                 .Include(q => q.Question_Tags)
                                 .ThenInclude(t => t.Tag)
                                 .FirstOrDefaultAsync(q => q.Id == id);
            return question;
        }

        public async Task<List<QuestionRating>> GetQuestionsRatingAsync()
        {
            return await _context.QuestionsRating.Include(qr => qr.Question)
                                                 .Include(qr => qr.User)
                                                 .ToListAsync();
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _context.Questions
                                 .Include(q => q.User)
                                 .Include(q => q.Answers)
                                 .Include(q => q.Ratings)
                                 .Include(q => q.Question_Tags)
                                 .ThenInclude(t => t.Tag)
                                 .ToListAsync();
        }

        public async Task<Question> UpdateQuestionAsync(int id, QuestionVM model)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return null;

            question.Title = model.Title;
            question.Description = model.Description;
            question.UserId = model.UserId;
            question.Modified = DateTime.Now;
            await _context.SaveChangesAsync();

            var question_tags = _context.QuestionTags.Where(qt => qt.QuestionId == question.Id);
            _context.QuestionTags.RemoveRange(question_tags);
            foreach (var tag in model.TagsId)
            {
                await _context.QuestionTags.AddAsync(new Question_Tag() { QuestionId = question.Id, TagId = tag });
            }
            await _context.SaveChangesAsync();

            return question;
        }
    }
}
