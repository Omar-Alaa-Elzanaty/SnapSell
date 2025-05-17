using System.Net;
using FluentValidation;
using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;

namespace SnapSell.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly IValidator<TRequest>[] _validators = validators.ToArray();

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Length == 0) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Count > 0)
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();

        if (validationFailures.Count > 0)
        {
            var errorDictionary = validationFailures.GetErrorsDictionary();
            var emptyResult = Activator.CreateInstance<TResponse>() ?? throw new InvalidOperationException(
                $"Could not create an instance of {typeof(TResponse)}. Make sure it has a parameterless constructor.");

            return (TResponse)(IResult)emptyResult.ToValidationErrors(
                errors: errorDictionary,
                statusCode: HttpStatusCode.BadRequest,
                message: "Validation Process is failed to current request.");
        }

        return await next();
    }
}