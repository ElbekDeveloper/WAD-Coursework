﻿using AutoMapper;
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
        private IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository repository, IMapper mapper)
        {
            _articleRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleResource>> GetArticles()
        {
            var articles = await _articleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArticleResource>>(articles);
        }

        public async Task<ArticleResource> AddArticle(string userId, AddArticleResource addArticleResource, CancellationToken cancellationToken)
        {
 
            var article = _mapper.Map<Article>(addArticleResource);
            article.CreatedDate = DateTime.UtcNow;
            article.AuthorId = userId;
            var createdArticle = await _articleRepository.AddAsync(article, cancellationToken);
            return _mapper.Map<ArticleResource>(createdArticle);
        }

        public async Task<ArticleResource> DeleteArticle(int id, CancellationToken cancellationToken = default)
        {
           var article = await _articleRepository.RemoveAsync(id, cancellationToken);
            return _mapper.Map<ArticleResource>(article);
        }

        public async Task<ArticleResource> GetArticle(int id, CancellationToken cancellationToken = default)
        {
            var article = await _articleRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<ArticleResource>(article);
        }

        public async Task<ArticleResource> UpdateArticle(string userId, int id,AddArticleResource articleResource, CancellationToken cancellationToken = default)
        {
            var article = _mapper.Map<Article>(articleResource);
            article.Id = id;
            article.AuthorId = userId;
            article.UpdatedDate = DateTime.UtcNow;
            await _articleRepository.UpdateAsync(article, cancellationToken);
            return await GetArticle(article.Id, cancellationToken);
        }
    }
}
