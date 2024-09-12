namespace Livraria.Core.Domain.Queries;

public interface IQueryHandler<TQuery, TOutput>
    where TOutput: class
    where TQuery: IQuery
{
    Task<TOutput> Handle(TQuery query, CancellationToken cancellationToken = default);
}