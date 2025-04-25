using FluentValidation;
using MediatR;
using SnapSell.Application.Exceptions;

namespace SnapSell.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>? _validator = validator;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null) return await next();

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
                return await next();

            var errors = validationResult.Errors
                .ConvertAll(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage));

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);

            return await next();
        }
    }
}
