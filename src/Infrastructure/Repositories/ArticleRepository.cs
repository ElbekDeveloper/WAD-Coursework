using Core.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public override async Task<IEnumerable<Article>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var articles = await _dbContext.Articles
                                     .Include(a => a.Author)
                                     .Include(a => a.Tags).ToListAsync(cancellationToken);

            return articles;
        }
    }
}
