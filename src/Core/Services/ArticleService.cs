using AutoMapper;
using Core.Interfaces;
using Core.Repositories;
using Core.Resources;
using Domain.Models;
using System;
using System.Collections;
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

        public async Task<ArticleResource> AddArticle(ArticleResource articleResource, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(articleResource);
            var createdArticle = await _repository.AddAsync(article, cancellationToken);
            return _mapper.Map<ArticleResource>(createdArticle);
        }
    }
}
