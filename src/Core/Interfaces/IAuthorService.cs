using Core.Resources;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResource>> GetAuthors();
        Task<AuthorResource> AddAuthor(AddAuthorResource authorResource, CancellationToken cancellationToken = default);
        Task<AuthorResource> DeleteAuthor(int id, CancellationToken cancellationToken = default);
        Task<AuthorResource> GetAuthor(int id, CancellationToken cancellationToken = default);
        Task<AuthorResource> UpdateAuthor(int id, AddAuthorResource authorResource, CancellationToken cancellationToken = default);
    }
}
