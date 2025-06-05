using FluentValidation;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

namespace SnapSell.Application.Features.product.Commands.AddVariantsToProduct;

public sealed class AddVariantsToProductCommandValidator : AbstractValidator<VariantDto>
{
    public AddVariantsToProductCommandValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.RegularPrice)
            .GreaterThanOrEqualTo(x => x.Price)
            .WithMessage("Regular price must be greater than or equal to sale price");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");

        RuleFor(x => x.Sku)
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters");
    }
}