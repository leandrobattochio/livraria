using System.Net;
using Livraria.Core.Application.Result;
using Livraria.Domain.Commands.Books;
using Livraria.Domain.Queries.Books;
using Livraria.Domain.Repository.Books;
using Livraria.IntegrationTests.Core;
using Newtonsoft.Json;
using Shouldly;
using Xunit.Abstractions;

namespace Livraria.IntegrationTests.Tests.Books;

public class BookIntegrationFixture : BaseIntegrationFixture
{

}

public class BookIntegrationTests(
    BookIntegrationFixture fixture,
    ITestOutputHelper testOutputHelper)
    : BaseIntegrationTest<BookIntegrationFixture>(fixture, testOutputHelper)
{
    [Theory]
    [InlineData("Livro 1", "incorrect")]
    public async Task ShouldCreateBook_WhenValidInput(string name, string publisher)
    {
        var cliente = CreateClient();

        var command = new CreateBookCommand(name, publisher, DateTime.UtcNow);

        var response = await cliente.PostAsync("api/book", command.ToPayload());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Theory]
    [InlineData("", "Editora Teste")]
    [InlineData("Livro teste", "")]
    public async Task ShouldNotCreateBook_WhenInvalidInput(string name, string publisher)
    {
        var cliente = CreateClient();

        var command = new CreateBookCommand(name, publisher, DateTime.UtcNow);

        var response = await cliente.PostAsync("api/book", command.ToPayload());

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task ShouldGetBook_WhenValidInput()
    {
        var cliente = CreateClient();

        var data = DateTime.UtcNow;
        var command = new CreateBookCommand("Livro Teste", "Editora teste", data);
        var response = await cliente.PostAsync("api/book", command.ToPayload());
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result<BookReadModel>>(content);
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        var bookId = result.Data.Id;
        
        
        var query = new GetBookQuery(bookId);
        var getResponse = await cliente.GetAsync($"api/book/{bookId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseResult = await getResponse.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<Result<BookReadModel>>(responseResult);

        getResult.ShouldNotBeNull();
        getResult.Data.ShouldNotBeNull();
        getResult.Data.Id.ShouldBe(bookId);
        getResult.Data.Name.ShouldBe("Livro Teste");
        getResult.Data.Publisher.ShouldBe("Editora teste");
    }
}