using FluentValidation;
using Livraria.Core.Application.Result;
using Livraria.Core.Domain.Queries;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Models;
using Livraria.Domain.Queries.Books;
using Livraria.Domain.Repository.Books;
using Livraria.Infrastructure;

namespace Livraria.Services.QueryHandlers.Books;

public class GetBookQueryHandler(
    IValidator<GetBookQuery> validator,
    IUnitOfWork<LivrariaDbContext> unitOfWork)
    : IQueryHandler<GetBookQuery, Result<BookReadModel>>
{
    public async Task<Result<BookReadModel>> Handle(GetBookQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BookReadModel>.BadRequest(validationResult);
        
        var repository = unitOfWork.GetRepository<IBookRepository, Book>();
        
        var book = await repository.GetBookReadModelAsync(query.BookId, cancellationToken);

        return book == null ? Result<BookReadModel>.NotFound() : Result<BookReadModel>.Ok(book);
    }
}