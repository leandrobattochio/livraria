using FluentValidation;
using Livraria.Core.Application.Result;
using Livraria.Core.Domain.Commands;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Commands.Books;
using Livraria.Domain.Models;
using Livraria.Domain.Repository.Books;
using Livraria.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Livraria.Services.CommandHandlers.Books;

public class CreateBookCommandHandler(
    IValidator<CreateBookCommand> validator,
    IUnitOfWork<LivrariaDbContext> unitOfWork,
    ILogger<CreateBookCommandHandler> logger)
    : ICommandHandler<CreateBookCommand, Result<BookReadModel>>
{
    public async Task<Result<BookReadModel>> Handle(CreateBookCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return Result<BookReadModel>.BadRequest(validationResult);
        
        var repository = unitOfWork.GetRepository<IBookRepository, Book>();

        var book = new Book(command.Name, command.Publisher, command.PublicationDate);

        try
        {
            await repository.AddAsync(book, cancellationToken);
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