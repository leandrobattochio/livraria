using Livraria.Core.Application.Result;
using Livraria.Core.Domain;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Commands.Books;
using Livraria.Domain.Models;
using Livraria.Domain.Repository.Books;
using Livraria.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Livraria.Services.CommandHandlers.Books;

public class CreateBookCommandHandler(
    CreateBookCommandValidator validator,
    IBookRepository bookRepository,
    IUnitOfWork<LivrariaDbContext> unitOfWork,
    ILogger<CreateBookCommandHandler> logger)
    : CommandHandler<CreateBookCommand, BookReadModel>(validator)
{
    protected override async Task<Result<BookReadModel>> Run(CreateBookCommand command,
        CancellationToken cancellationToken = default)
    {
        var book = new Book(command.Name, command.Publisher, command.PublicationDate);

        try
        {
            await bookRepository.AddAsync(book, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result<BookReadModel>.Created(
                new BookReadModel(book.Id, book.Name, book.Publisher, book.PublicationDate));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar livro");
            return Result<BookReadModel>.InternalServerError("Erro ao criar livro!");
        }
    }
}