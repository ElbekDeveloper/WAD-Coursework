using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IAuthorRepository<AuthorType>
    {
        Task<IEnumerable<Article>> GetArticlesByUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuthorType>> GetAuthorsAsync(CancellationToken cancellationToken =default);
        Task<AuthorType> DeleteAuthorAsync(string id, CancellationToken cancellationToken = default);
        Task<int> CountAuthorsAsync(CancellationToken cancellationToken = default);
    }
}
