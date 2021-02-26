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
    public class ArticleService : IArticleService
    {
        private IArticleRepository _repository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleResource>> GetArticles()
        {
            var article = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArticleResource>>(article);
        }

        public async Task<ArticleResource> AddArticle(AddArticleResource addArticleResource, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(addArticleResource);
            article.CreatedDate = DateTime.UtcNow;
            var createdArticle = await _repository.AddAsync(article, cancellationToken);
            return _mapper.Map<ArticleResource>(createdArticle);
        }

        public async Task<ArticleResource> DeleteArticle(int id, CancellationToken cancellationToken = default)
        {
           var article = await _repository.RemoveAsync(id, cancellationToken);
            return _mapper.Map<ArticleResource>(article);
        }

        public async Task<ArticleResource> GetArticle(int id, CancellationToken cancellationToken = default)
        {
            var article = await _repository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<ArticleResource>(article);
        }

        public async Task<ArticleResource> UpdateArticle(int id,AddArticleResource articleResource, CancellationToken cancellationToken = default)
        {
           
            var article = _mapper.Map<Article>(articleResource);
            article.Id = id;
            article.UpdatedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(article, cancellationToken);
            return await GetArticle(article.Id, cancellationToken);
        }
    }
}
