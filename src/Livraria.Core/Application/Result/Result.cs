using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Livraria.Core.Application.Result;

public class Result(HttpStatusCode statusCode) : ICommandResult
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public bool IsSuccess => Errors.Count == 0;
    public List<ResultError> Errors { get; } = [];

    public static Result BadRequest(ValidationResult validationResult)
    {
        var result = new Result(HttpStatusCode.BadRequest);

        validationResult.Errors
            .Select(c => new ResultError(c.PropertyName, c.ErrorMessage))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }

    public static Result BadRequest(IEnumerable<IdentityError> identityErrors)
    {
        var result = new Result(HttpStatusCode.BadRequest);

        identityErrors
            .Select(c => new ResultError(c.Code, c.Description))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }

    public static Result BadRequest(string errorMessage, string? propertyName = null)
    {
        var error = new ValidationResult([
            new ValidationFailure(propertyName, errorMessage)
        ]);
        return BadRequest(error);
    }

    public static Result Forbidden() => new(HttpStatusCode.Forbidden);
    public static Result NotFound() => new(HttpStatusCode.NotFound);
    public static Result NoContent() => new(HttpStatusCode.NoContent);

    public static Result InternalServerError(string errorMessage, string? propertyName = null)
    {
        var error = new ValidationResult([
            new ValidationFailure(propertyName, errorMessage)
        ]);
        return InternalServerError(error);
    }

    protected static Result InternalServerError(ValidationResult validationResult)
    {
        var result = new Result(HttpStatusCode.InternalServerError);

        validationResult.Errors
            .Select(c => new ResultError(c.PropertyName, c.ErrorMessage))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }
}

public class Result<TValue>(HttpStatusCode statusCode, TValue? data = default) : Result(statusCode)
{
    public TValue? Data { get; } = data;

    public new static Result<TValue> Forbidden() => new(HttpStatusCode.Forbidden);
    public new static Result<TValue> NoContent() => new(HttpStatusCode.NoContent);
    public new static Result<TValue> NotFound() => new(HttpStatusCode.NotFound);
    public static Result<TValue> Ok(TValue? value = default) => new(HttpStatusCode.OK, value);
    public static Result<TValue> Created(TValue value) => new(HttpStatusCode.Created, value);

    public new static Result<TValue> BadRequest(ValidationResult validationResult)
    {
        var result = new Result<TValue>(HttpStatusCode.BadRequest);

        validationResult.Errors
            .Select(c => new ResultError(c.PropertyName, c.ErrorMessage))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }

    public new static Result<TValue> BadRequest(IEnumerable<IdentityError> identityErrors)
    {
        var result = new Result<TValue>(HttpStatusCode.BadRequest);

        identityErrors
            .Select(c => new ResultError(c.Code, c.Description))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }

    public new static Result<TValue> BadRequest(string errorMessage, string? propertyName = null)
    {
        var error = new ValidationResult([
            new ValidationFailure(propertyName, errorMessage)
        ]);
        return BadRequest(error);
    }

    public new static Result<TValue> InternalServerError(string errorMessage, string? propertyName = null)
    {
        var error = new ValidationResult([
            new ValidationFailure(propertyName, errorMessage)
        ]);
        return InternalServerError(error);
    }

    public new static Result<TValue> InternalServerError(ValidationResult validationResult)
    {
        var result = new Result<TValue>(HttpStatusCode.InternalServerError);

        validationResult.Errors
            .Select(c => new ResultError(c.PropertyName, c.ErrorMessage))
            .ToList()
            .ForEach(result.Errors.Add);

        return result;
    }
}