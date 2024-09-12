using Livraria.Core.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Livraria.Core.Infrastructure;

public class UnitOfWork<Context>(
    IServiceProvider serviceProvider,
    Context context) : IUnitOfWork<Context>
    where Context : DbContext
{
    public Context DbContext => context;


    public Repository GetRepository<Repository, Entity>()
        where Repository : class, IRepository<Entity>
        where Entity : class, IBaseEntity, IAggregateRoot
    {
        return serviceProvider.GetRequiredService<Repository>()
               ?? throw new ArgumentException("business repository not found");
    }

    public TC GetUserRepository<TC>() where TC : class
    {
        return serviceProvider.GetRequiredService<TC>();
    }

    public SignInManager<TC> GetSignInManager<TC>() where TC : class
    {
        return serviceProvider.GetRequiredService<SignInManager<TC>>();
    }


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