
using Core.Resources;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public  interface IArticleService
    {
        Task<IEnumerable<ArticleResource>> GetArticles();
        Task<ArticleResource> AddArticle(ArticleResource articleResource, CancellationToken cancellationToken= default);
    }
}
