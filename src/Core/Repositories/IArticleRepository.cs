using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IArticleRepository : IRepository<Article, int>
    {

    }
}
