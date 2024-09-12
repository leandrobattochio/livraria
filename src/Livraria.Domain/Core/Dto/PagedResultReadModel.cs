namespace Livraria.Domain.Core.Dto;

public record PagedResultReadModel<TE>(
    List<TE> Itens,
    int PageCount,
    int PageSize,
    int CurrentPage,
    int RowCount);