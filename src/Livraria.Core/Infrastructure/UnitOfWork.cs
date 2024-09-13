using Microsoft.EntityFrameworkCore;

namespace Livraria.Core.Infrastructure;

public class UnitOfWork<Context>(Context context) : IUnitOfWork<Context>
    where Context : DbContext
{
    public Context DbContext => context;
    
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContextTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await context.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await dbContextTransaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}