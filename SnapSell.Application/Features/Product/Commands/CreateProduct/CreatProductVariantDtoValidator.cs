using FluentValidation;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed class CreatProductVariantDtoValidator : AbstractValidator<CreatProductVariantDto>
{
    public CreatProductVariantDtoValidator()
    {
        RuleFor(x => x.SizeId).NotEmpty().WithMessage("SizeId Is Required.");

        RuleFor(x => x.Color)
            .MaximumLength(30).WithMessage("Color must not exceed 30 characters")
            .When(x => !string.IsNullOrEmpty(x.Color));

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.CostPrice)
            .NotNull().WithMessage("CoastPrice is required")
            .GreaterThan(0).WithMessage("CoastPrice must be greater than 0");

        RuleFor(x => x.SalePrice)
            .LessThan(x => x.Price)
            .When(x => x.SalePrice > x.Price)
            .WithMessage("Sale price must be less than price");

        RuleFor(x => x.Quantity)
            .NotNull().WithMessage("Quantity is required")
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or more");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("Variant SKU is required")
            .MaximumLength(50).WithMessage("Variant SKU must not exceed 50 characters");
    }
}