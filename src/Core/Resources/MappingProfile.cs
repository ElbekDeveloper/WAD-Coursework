using AutoMapper;
using Domain.Models;

namespace Core.Resources
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to Api Resources
            CreateMap<Article, ArticleResource>();
            CreateMap<Author, AuthorResource>().ReverseMap();


            //From Resource to Domain model
            CreateMap<AddArticleResource, Article>();
            CreateMap<ArticleResource, Article>()
                .ForMember(a => a.AuthorId, opt => opt.MapFrom(ar => ar.Author.Id));
        }
    }
}
