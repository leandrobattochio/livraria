using System.Net;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Commands.Books;
using Livraria.Infrastructure;
using Livraria.Infrastructure.Repository;
using Livraria.Services.CommandHandlers.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Livraria.Tests.Services.Commands;

public class CreateBookCommandHandlerTests
{
    private readonly CreateBookCommandHandler _handler;

    public CreateBookCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<LivrariaDbContext>()
            .UseInMemoryDatabase(databaseName: "LivrariaInMemoryDb")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        var dbContext = new LivrariaDbContext(options);
        var unitOfWork = new UnitOfWork<LivrariaDbContext>(dbContext);
        var bookRepository = new BookRepository(dbContext);
        var validator = new CreateBookCommandValidator();
        var loggerMock = new Mock<ILogger<CreateBookCommandHandler>>();

        _handler = new CreateBookCommandHandler(validator, bookRepository, unitOfWork, loggerMock.Object);
    }

    [Fact]
    public async Task ShouldCreateBook_WhenValidCommand()
    {
        // Arrange
        var command = new CreateBookCommand("Livro teste", "Editora Teste", DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Id.ShouldNotBe(0);
    }
}