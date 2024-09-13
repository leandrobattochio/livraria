using Livraria.Core.Application.Result;
using Livraria.Core.Domain;
using Livraria.Domain.Queries.Books;
using Livraria.Domain.Repository.Books;

namespace Livraria.Services.QueryHandlers.Books;

public class GetBookQueryHandler(
    GetBookQueryValidator validator,
    IBookRepository bookRepository)
    : QueryHandler<GetBookQuery, BookReadModel>(validator)
{
    protected override async Task<Result<BookReadModel>> Run(GetBookQuery query,
        CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetBookReadModelAsync(query.BookId, cancellationToken);
        return book == null ? Result<BookReadModel>.NotFound() : Result<BookReadModel>.Ok(book);
    }
}