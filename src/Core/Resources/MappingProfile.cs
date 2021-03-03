using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Resources
{
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Domain to Api Resources
        CreateMap<Article, ArticleResource>();
        CreateMap<IdentityUser, AuthorResource>()
        .ForMember(ar => ar.Name, opt => opt.MapFrom(u => u.UserName));

        //From Resource to Domain model
        CreateMap<AddArticleResource, Article>();
        CreateMap<AuthorResource, IdentityUser>()
        .ForMember(u => u.UserName, opt => opt.MapFrom(ar => ar.Name))
        .ForMember(u => u.Id, opt => opt.MapFrom(ar => ar.Id));
        CreateMap<ArticleResource, Article>()
        .ForMember(a => a.AuthorId, opt => opt.MapFrom(ar => ar.Author.Id));
    }
}
}
