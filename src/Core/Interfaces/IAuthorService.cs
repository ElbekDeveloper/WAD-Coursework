using Core.Resources;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces {
  public interface IAuthorService {
    Task<IEnumerable<AuthorResource>> GetAuthors(
        CancellationToken cancellationToken = default);

    Task<AuthorResource> DeleteAuthor(
        string id, CancellationToken cancellationToken = default);
    Task<int> CountAuthors(CancellationToken cancellationToken = default);
  }
}
