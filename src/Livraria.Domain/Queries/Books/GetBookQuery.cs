using FluentValidation;
using Livraria.Core.Domain.Queries;

namespace Livraria.Domain.Queries.Books;

public record GetBookQuery(int BookId) : QueryBase;



public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
{
    public GetBookQueryValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty();
    }
}