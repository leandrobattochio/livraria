using FluentValidation;
using Livraria.Core.Application.Result;
using Livraria.Core.Domain.Commands;

namespace Livraria.Core.Domain;

public abstract class CommandHandler<TCommand, TOutput>(IValidator<TCommand> validator)
    : ICommandHandler<TCommand, Result<TOutput>>
    where TCommand : ICommand
    where TOutput : class
{
    protected abstract Task<Result<TOutput>> Run(TCommand command, CancellationToken cancellationToken = default);

    public async Task<Result<TOutput>> Handle(TCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await RequestHelper.ValidateRequest(validator, request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<TOutput>.BadRequest(validationResult);

        return await Run(request, cancellationToken);
    }
}

public abstract class CommandHandler<TCommand>(IValidator<TCommand> validator)
    : ICommandHandler<TCommand, Result>
    where TCommand : ICommand
{
    protected abstract Task<Result> Run(TCommand command, CancellationToken cancellationToken = default);

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await RequestHelper.ValidateRequest(validator, request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.BadRequest(validationResult);

        return await Run(request, cancellationToken);
    }
}