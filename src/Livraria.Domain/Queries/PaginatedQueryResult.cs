using Livraria.Core.Domain.Queries;

namespace Livraria.Domain.Queries;

public record PaginatedQueryResult<TE>(
    List<TE> Itens,
    int PageCount,
    int PageSize,
    int CurrentPage,
    int RowCount) : IPaginatedQueryResult;