using Livraria.Core.Domain.Entity;

namespace Livraria.Domain.Core;

public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; init; }
}