using AutoMapper;
using Core.Interfaces;
using Core.Repositories;
using Core.Resources;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
{

    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _repository;
        private readonly IMapper _mapper;


        public AuthorService(IAuthorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AuthorResource> AddAuthor(AddAuthorResource authorResource, CancellationToken cancellationToken = default)
        {
            var author = _mapper.Map<Author>(authorResource);
            var result = await _repository.AddAsync(author, cancellationToken);
            return _mapper.Map<AuthorResource>(result);
        }

        public async Task<AuthorResource> DeleteAuthor(int id, CancellationToken cancellationToken = default)
        {
           var deletedAuthor = await _repository.RemoveAsync(id, cancellationToken);
            return _mapper.Map<AuthorResource>(deletedAuthor);
        }

        public async Task<AuthorResource> GetAuthor(int id, CancellationToken cancellationToken = default)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<AuthorResource>(author);
        }

        public async Task<IEnumerable<AuthorResource>> GetAuthors()
        {
            var authors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorResource>>(authors);
        }

        public async Task<AuthorResource> UpdateAuthor(int id, AddAuthorResource authorResource, CancellationToken cancellationToken = default)
        {
            var author = _mapper.Map<Author>(authorResource);
            author.Id = id;
            await _repository.UpdateAsync(author, cancellationToken);
            return await GetAuthor(author.Id, cancellationToken);
        }
        
    }
}
