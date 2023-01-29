using Microsoft.EntityFrameworkCore;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public class TagService : ITagService
    {
        private readonly ForumDbContext _context;

        public TagService(ForumDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateTagAsync(TagVM model)
        {
            var tag = new Tag() { Name = model.Name, Description = model.Description };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await _context.Tags.Include(t => t.Tag_Questions).ThenInclude(qt => qt.Question).ThenInclude(q => q.Answers).ToListAsync();
        }
        public async Task<Tag> GetTagByNameAsync(string tag)
        {
            return await _context.Tags.Include(t => t.Tag_Questions).ThenInclude(qt => qt.Question).ThenInclude(q => q.Answers).FirstOrDefaultAsync(t => t.Name == tag);
        }
    }
}
