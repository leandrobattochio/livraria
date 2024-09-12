namespace Livraria.Core.Domain;

public abstract record RequestBase
{    
    protected Guid IdempotencyId { get; init; } = Guid.NewGuid();
}