using FluentValidation;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed class CreatProductCommandValidator : AbstractValidator<CreatProductCommand>
{
    public CreatProductCommandValidator()
    {
        RuleFor(c => c.BrandId).NotEmpty().WithMessage("BrandId is required");

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage("English name is required")
            .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage("Arabic name is required")
            .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

        RuleFor(x => x.ShippingType)
            .Must(x => Enum.IsDefined(typeof(ShippingType), x))
            .WithMessage("Invalid shipping type.");

        RuleFor(x => x.ProductStatus)
            .Must(x => Enum.IsDefined(typeof(ProductStatus), x))
            .WithMessage("Invalid product status.");
    }
}