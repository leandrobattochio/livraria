using Livraria.Core.Domain.Entity;

namespace Livraria.Core.Infrastructure;

public interface IRepository<E> : ITransientDependency
    where E : class, IBaseEntity, IAggregateRoot;