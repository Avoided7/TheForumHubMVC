namespace TheForumHubMVC.Models
{
    public class AnswerRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        // Relations
        public string UserId { get; set; }
        public User User { get; set; }
        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }
    }
}
