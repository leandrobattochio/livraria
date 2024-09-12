using FluentValidation;
using Livraria.Core.Application.Result;
using Livraria.Core.Domain.Queries;

namespace Livraria.Core.Domain;

public abstract class QueryHandler<TQuery, TOutput>(IValidator<TQuery> validator)
    : IQueryHandler<TQuery, Result<TOutput>>
    where TQuery : IQuery
    where TOutput : class
{
    protected abstract Task<Result<TOutput>> Run(TQuery query, CancellationToken cancellationToken = default);

    public async Task<Result<TOutput>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await RequestHelper.ValidateRequest(validator, query, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TOutput>.BadRequest(validationResult);

        return await Run(query, cancellationToken);
    }
}

public abstract class QueryHandler<TQuery>(IValidator<TQuery> validator)
    : IQueryHandler<TQuery, Result>
    where TQuery : IQuery
{
    protected abstract Task<Result> Run(TQuery query, CancellationToken cancellationToken = default);

    public async Task<Result> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await RequestHelper.ValidateRequest(validator, query, cancellationToken);
        if (!validationResult.IsValid)
            return Result.BadRequest(validationResult);

        return await Run(query, cancellationToken);
    }
}