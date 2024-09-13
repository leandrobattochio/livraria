using System.Net;
using Livraria.Core.Infrastructure;
using Livraria.Domain.Commands.Books;
using Livraria.Infrastructure;
using Livraria.Infrastructure.Repository;
using Livraria.Services.CommandHandlers.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Livraria.Tests.Services.Commands;

public class CreateBookCommandHandlerTests
{
    private readonly CreateBookCommandHandler _handler;

    public CreateBookCommandHandlerTests()
    {
        // Configurando o Mock para UserManager

        var validator = new CreateBookCommandValidator();

        var options = new DbContextOptionsBuilder<LivrariaDbContext>()
            .UseInMemoryDatabase(databaseName: "ClicheriaTestDb")
            .Options;
        var dbContext = new LivrariaDbContext(options);

        // Configurando o Mock para IUnitOfWork
        var unitOfWork = new Mock<IUnitOfWork<LivrariaDbContext>>();
        var bookRepository = new BookRepository(unitOfWork.Object);

        unitOfWork.Setup(c => c.DbContext).Returns(dbContext);

        var loggerMock = new Mock<ILogger<CreateBookCommandHandler>>();

        _handler = new CreateBookCommandHandler(validator, bookRepository, unitOfWork.Object, loggerMock.Object);
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