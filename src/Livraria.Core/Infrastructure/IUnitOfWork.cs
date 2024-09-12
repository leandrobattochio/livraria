using Livraria.Core.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Core.Infrastructure;

public interface IUnitOfWork<Context>
    where Context : DbContext
{
    Context DbContext { get; }

    Repository GetRepository<Repository, Entity>()
        where Repository : class, IRepository<Entity>
        where Entity : class, IBaseEntity, IAggregateRoot;

    TC GetUserRepository<TC>()
        where TC : class;

    SignInManager<TC> GetSignInManager<TC>()
        where TC : class;

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}