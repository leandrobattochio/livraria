using System.Net;
using Livraria.Core.Application.Result;
using Microsoft.AspNetCore.Mvc;

namespace Livraria.Core.Application;

[ApiController]
[Route("api/[controller]")]
public abstract class LivrariaAppController : ControllerBase
{
    protected IActionResult Match<T>(T typedResult,
        Func<T, IActionResult> success,
        Func<IActionResult>? error = null)
        where T : IRequestResult
    {
        if (typedResult.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.Ambiguous)
            return success(typedResult);

        if (error != null)
            return error();

        return typedResult.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(),
            HttpStatusCode.Unauthorized => Unauthorized(),
            HttpStatusCode.BadRequest => BadRequest(typedResult),
            _ => new ObjectResult(typedResult)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };
    }
}