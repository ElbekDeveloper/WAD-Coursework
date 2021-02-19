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
    Task AddAsync(TModel obj, CancellationToken cancellationToken = default);
    Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(TId id, TModel obj, CancellationToken cancellationToken = default);
    Task<TModel> RemoveAsync(TId id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Finds list of objects based on the predicate
    /// </summary>
    /// <param name="predicate">Lambda expression to filter objects</param>
    /// <returns></returns>
    Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> predicate);
}
}
