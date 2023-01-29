namespace TheForumHubMVC.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relations
        public IEnumerable<Question_Tag> Tag_Questions { get; set; }
    }
}
