using FluentValidation;
using MediatR;
using SnapSell.Domain.Extnesions;

namespace SnapSell.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return await next();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next();

        var errors = validationResult.Errors.GetErrorsDictionary();

        if (errors.Any())
            throw new InvalidOperationException("An Error in validation behavior.");

        return await next();
    }
}