namespace Livraria.Core.Domain.Queries;

public interface IPaginatedQuery
{
    int Page { get; }
    int PerPage { get; }
}