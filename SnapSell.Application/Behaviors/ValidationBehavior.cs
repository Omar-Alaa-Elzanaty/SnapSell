using FluentValidation;
using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;

namespace SnapSell.Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null) return await next();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid) return await next();
        var errors = validationResult.Errors.GetErrorsDictionary();

        // Handle Result<T> response type
        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            // Create the appropriate Result<T> via reflection
            var method = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod("ValidationBehavoirFailure");

            var result = method.Invoke(null, new object[] { errors, "Validation failed" });

            return (TResponse)result!;
        }

        throw new ValidationException(validationResult.Errors);
    }
}