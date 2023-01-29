using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.ViewModels.Account
{
    public class ProfileVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] LogoImg { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Notification { get; set; }

        public IEnumerable<Models.Answer> Answers { get; set; }
        public IEnumerable<Models.Question> Questions { get; set; }
        public IEnumerable<QuestionRating>? QuestionRatings { get; set; }
        public IEnumerable<AnswerRating> AnswerRatings { get; set; }
    }
}
