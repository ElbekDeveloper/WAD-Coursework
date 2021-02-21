using Core.Repositories;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    
    public abstract class Repository<TModel> : IRepository<TModel, int>
        where TModel : class, IEntity<int>
    {
        protected readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public virtual async Task<TModel> AddAsync(TModel entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TModel>().ToListAsync(cancellationToken);
        }

        public virtual async Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<TModel>().FindAsync(keyValues, cancellationToken);
        }

        public virtual async Task<TModel> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity =  await GetByIdAsync(id);
            _dbContext.Set<TModel>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task AddRangeAsync(ICollection<TModel> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TModel>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
