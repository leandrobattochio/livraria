using FluentValidation;
using FluentValidation.Results;

namespace Livraria.Core.Domain;

public static class RequestHelper
{
    public static async Task<ValidationResult> ValidateRequest<TRequest>(IValidator<TRequest> validator,
        TRequest request, CancellationToken cancellationToken = default)
        where TRequest: IRequest
    {
        return await validator.ValidateAsync(request, cancellationToken);
    }
}