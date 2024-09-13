using Microsoft.EntityFrameworkCore;

namespace Livraria.Core.Infrastructure;

public interface IUnitOfWork<Context>
    where Context : DbContext
{
    Context DbContext { get; }
    
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}