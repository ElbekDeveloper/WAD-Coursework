using Core.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public  interface IArticleService
    {
        Task<IEnumerable<ArticleResource>> GetArticles();

        Task<ArticleResource> AddArticle(string userId, AddArticleResource articleResource, CancellationToken cancellationToken= default);
        Task<ArticleResource> DeleteArticle(int id, CancellationToken cancellationToken = default);
        Task<ArticleResource> GetArticle(int id, CancellationToken cancellationToken = default);
        Task<ArticleResource> UpdateArticle(string userId,int id, AddArticleResource articleResource, CancellationToken cancellationToken = default);
    }
}
