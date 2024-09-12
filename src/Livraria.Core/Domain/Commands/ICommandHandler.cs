namespace Livraria.Core.Domain.Commands;

public interface ICommandHandler<TCommand, TOutput>
    where TOutput: class
    where TCommand: ICommand
{
    Task<TOutput> Handle(TCommand request, CancellationToken cancellationToken = default);
}