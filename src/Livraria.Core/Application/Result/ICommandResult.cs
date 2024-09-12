using System.Net;

namespace Livraria.Core.Application.Result;

public interface ICommandResult
{
    bool IsSuccess { get; }
    List<ResultError> Errors { get; }
    HttpStatusCode StatusCode { get; }
}