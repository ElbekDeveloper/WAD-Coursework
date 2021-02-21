using Core.Interfaces;
using Core.Repositories;
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

        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            return await _repository.GetAllAsync();
        }
    }
}
