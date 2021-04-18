using Core.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<Article> AddAsync(Article entity, CancellationToken cancellationToken = default)
        {
            var article = await base.AddAsync(entity, cancellationToken);
            return await GetByIdAsync(article.Id, cancellationToken);
        }

        public override async Task<IEnumerable<Article>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var articles = await _dbContext.Articles
                                     .Include(a => a.Author)
                                     .ToListAsync(cancellationToken);

            return articles;
        }

       

        public override async Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var article = await _dbContext.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            return article;
        }
    }
}
