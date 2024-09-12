using Livraria.Core.Application;
using Livraria.Domain.Commands.Books;
using Livraria.Domain.Queries.Books;
using Livraria.Services.CommandHandlers.Books;
using Livraria.Services.QueryHandlers.Books;
using Microsoft.AspNetCore.Mvc;

namespace Livraria.ApiHost.Controllers;

public class BookController : LivrariaAppController
{
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand input,
        [FromServices] CreateBookCommandHandler handler,
        CancellationToken cancellationToken = default)
    { 
        var result = await handler.Handle(input, cancellationToken);

        return Match(result, success: (r) => Created(nameof(GetBook), result));
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBook(int id, 
        [FromServices] GetBookQueryHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBookQuery(id);
        var result = await handler.Handle(query, cancellationToken);

        return Match(result, success: Ok);
    }
}