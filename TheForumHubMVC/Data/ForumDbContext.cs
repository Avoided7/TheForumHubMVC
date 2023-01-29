using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data
{
    public class ForumDbContext : IdentityDbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question_Tag>().HasKey(qt => new { qt.TagId, qt.QuestionId });

            builder.Entity<Answer>().HasKey(a => a.Id);

            builder
                .Entity<Question_Tag>()
                .HasOne(q => q.Question)
                .WithMany(qt => qt.Question_Tags)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasForeignKey(qi => qi.QuestionId);

            builder
                .Entity<Question_Tag>()
                .HasOne(t => t.Tag)
                .WithMany(tq => tq.Tag_Questions)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasForeignKey(ti => ti.TagId);
            base.OnModelCreating(builder);
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Question_Tag> QuestionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionRating> QuestionsRating { get; set; }
        public DbSet<AnswerRating> AnswersRating { get; set; }
    }
}
