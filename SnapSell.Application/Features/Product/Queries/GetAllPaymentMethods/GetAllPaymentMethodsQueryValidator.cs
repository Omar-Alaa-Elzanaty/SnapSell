using FluentValidation;

namespace SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;

public sealed class GetAllPaymentMethodsQueryValidator : AbstractValidator<GetAllPaymentMethodsQuery>
{
    public GetAllPaymentMethodsQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Seller ID must be a valid GUID.");
    }
}