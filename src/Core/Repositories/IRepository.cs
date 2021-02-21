using Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRepository<TModel, TId> where TModel : IEntity<TId>
    {
        Task<TModel> AddAsync(TModel entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default);
        Task<TModel> RemoveAsync(TId id, CancellationToken cancellationToken = default);
        Task AddRangeAsync(ICollection<TModel> entities, CancellationToken cancellationToken = default);
    }
}
