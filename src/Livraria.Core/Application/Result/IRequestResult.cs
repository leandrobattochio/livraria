using System.Net;

namespace Livraria.Core.Application.Result;

public interface IRequestResult
{
    bool IsSuccess { get; }
    List<ResultError> Errors { get; }
    HttpStatusCode StatusCode { get; }
}