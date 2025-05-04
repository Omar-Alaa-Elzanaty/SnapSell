using FluentValidation;
using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Extnesions;
using System.Net;

namespace SnapSell.Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
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

        var errorResult = new ErrorResult(
            message: "Validation failed",
            errors: errors,
            statusCode: HttpStatusCode.BadRequest 
        );

        return (TResponse)(object)errorResult;
    }
}