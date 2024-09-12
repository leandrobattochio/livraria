using FluentValidation;
using Livraria.Core.Application.Result;
using Livraria.Core.Domain;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Models;
using Livraria.Domain.Queries.Books;
using Livraria.Domain.Repository.Books;
using Livraria.Infrastructure;

namespace Livraria.Services.QueryHandlers.Books;

public class GetBookQueryHandler(
    IValidator<GetBookQuery> validator,
    IUnitOfWork<LivrariaDbContext> unitOfWork)
    : QueryHandler<GetBookQuery, BookReadModel>(validator)
{
    protected override async Task<Result<BookReadModel>> Run(GetBookQuery query,
        CancellationToken cancellationToken = default)
    {
        var repository = unitOfWork.GetRepository<IBookRepository, Book>();

        var book = await repository.GetBookReadModelAsync(query.BookId, cancellationToken);

        return book == null ? Result<BookReadModel>.NotFound() : Result<BookReadModel>.Ok(book);
    }
}