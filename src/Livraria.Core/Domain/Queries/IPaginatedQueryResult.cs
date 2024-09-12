namespace Livraria.Core.Domain.Queries;

public interface IPaginatedQueryResult
{
    int PageCount { get; }
    int PageSize { get; }
    int CurrentPage { get; }
    int RowCount { get; }
}