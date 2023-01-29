using AutoMapper;
using TheForumHubMVC.Data.ViewModels.Account;
using TheForumHubMVC.Data.ViewModels.Question;
using TheForumHubMVC.Models;

namespace TheForumHubMVC.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, ProfileVM>();
            CreateMap<User, EditVM>();
            CreateMap<User, DeleteVM>();
            CreateMap<Question, QuestionVM>();
        }
    }
}
