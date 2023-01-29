using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetTagsAsync();
        Task<Tag> CreateTagAsync(TagVM model);
        Task<Tag> GetTagByNameAsync(string tag);
    }
}
