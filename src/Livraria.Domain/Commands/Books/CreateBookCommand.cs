using FluentValidation;
using Livraria.Core.Domain.Commands;

namespace Livraria.Domain.Commands.Books;

public record CreateBookCommand(string Name, string Publisher, DateTime PublicationDate) : CommandBase;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();
    }
}