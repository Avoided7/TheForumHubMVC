using Microsoft.AspNetCore.Identity;

namespace TheForumHubMVC.Models
{
    public class User : IdentityUser
    {
        public byte[] LogoImg { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool OnNotification { get; set; } = false;
        public string VerificationToken { get; set; } = null;
        public DateTime? VerificatedAt { get; set; } = null;
        // Relations
        public IEnumerable<Answer> Answers { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<QuestionRating> QuestionRatings { get; set; }
        public IEnumerable<AnswerRating> AnswerRatings { get; set; }
    }
}
