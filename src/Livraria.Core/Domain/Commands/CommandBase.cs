namespace Livraria.Core.Domain.Commands;

public abstract record CommandBase : RequestBase, ICommand;