namespace TheForumHubMVC.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SendedAt { get; set; }
        public DateTime? Modified { get; set; } = null;

        // Relations
        public IEnumerable<AnswerRating> Ratings { get; set; }
        public int? QuestionId { get; set; }
        public Question Question { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
    }
}
