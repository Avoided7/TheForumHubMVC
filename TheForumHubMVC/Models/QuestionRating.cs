namespace TheForumHubMVC.Models
{
    public class QuestionRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        // Relations
        public string UserId { get; set; }
        public User User { get; set; }
        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
