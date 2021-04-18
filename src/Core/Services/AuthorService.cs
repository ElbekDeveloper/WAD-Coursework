using AutoMapper;
using Core.Interfaces;
using Core.Repositories;
using Core.Resources;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
{

    public class AuthorService : IAuthorService
    {
        private IAuthorRepository<IdentityUser> _repository;
        private readonly IMapper _mapper;


        public AuthorService(IAuthorRepository<IdentityUser> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        
        public async Task<AuthorResource> DeleteAuthor(string id, CancellationToken cancellationToken = default)
        {
           var deletedAuthor = await _repository.DeleteAuthorAsync(id, cancellationToken);
            return _mapper.Map<AuthorResource>(deletedAuthor);
        }

        public async Task<IEnumerable<AuthorResource>> GetAuthors(CancellationToken cancellationToken = default)
        {
            var authors = await _repository.GetAuthorsAsync();
            return _mapper.Map<IEnumerable<IdentityUser>, IEnumerable<AuthorResource>>(authors);
        }

        public async Task<int> CountAuthors(CancellationToken cancellationToken = default)
        {
            return await _repository.CountAuthorsAsync(cancellationToken);
        }

        public async Task<IEnumerable<ArticleResource>> GetArticlesByUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var articles = await _repository.GetArticlesByUserAsync(userId, cancellationToken);
            return _mapper.Map<IEnumerable<ArticleResource>>(articles);
        }
    }
}
