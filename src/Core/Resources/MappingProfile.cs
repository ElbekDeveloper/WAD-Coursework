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
            CreateMap<Author, AuthorResource>();
            CreateMap<Tag, TagResource>();

            //From Resource to Domain model

        }
    }
}
