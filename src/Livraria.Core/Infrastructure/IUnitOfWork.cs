using Microsoft.EntityFrameworkCore;

namespace Livraria.Core.Infrastructure;

public interface IUnitOfWork<Context>
    where Context : DbContext
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}