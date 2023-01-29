namespace TheForumHubMVC.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public DateTime Asked { get; set; }
        public DateTime? Modified { get; set; } = null;

        // Relations
        public IEnumerable<QuestionRating> Ratings { get; set; }
        public IEnumerable<Question_Tag> Question_Tags { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
