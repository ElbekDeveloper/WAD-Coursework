
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public  interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticles();
    }
}
