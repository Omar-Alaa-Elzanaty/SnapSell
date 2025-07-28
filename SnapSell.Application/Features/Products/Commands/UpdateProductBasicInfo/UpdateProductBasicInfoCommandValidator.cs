using FluentValidation;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Products.Commands.UpdateProductBasicInfo;

public class UpdateProductBasicInfoCommandValidator
    : AbstractValidator<UpdateProductBasicInfoCommand>
{
    public UpdateProductBasicInfoCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .WithMessage("Brand ID is required");

        RuleFor(x => x.CategoryIds)
            .NotEmpty()
            .WithMessage("At least one category is required")
            .Must(c => c.Distinct().Count() == c.Count)
            .WithMessage("Duplicate category IDs are not allowed");

        RuleFor(x => x.EnglishName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("English name is required (max 200 chars)");

        RuleFor(x => x.ArabicName)
            .NotEmpty()
            .WithMessage("Arabic name is required.");

        RuleFor(x => x.EnglishDescription)
            .NotEmpty()
            .WithMessage("English description is required.");

        RuleFor(x => x.ArabicDescription)
            .NotEmpty()
            .WithMessage("Arabic description is required.");

        RuleFor(x => x.MinDeliveryDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum delivery days cannot be negative");

        RuleFor(x => x.MaxDeliveryDays)
            .GreaterThanOrEqualTo(x => x.MinDeliveryDays)
            .When(x => x.MinDeliveryDays >= 0)
            .WithMessage("Max delivery days must be >= min delivery days");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue)
            .WithMessage("Price must be positive when specified");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .When(x => x.SalePrice.HasValue)
            .WithMessage("Sale price must be positive when specified")
            .LessThanOrEqualTo(x => x.Price)
            .When(x => x.SalePrice.HasValue && x.Price.HasValue)
            .WithMessage("Sale price cannot exceed regular price");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0)
            .When(x => x.CostPrice.HasValue)
            .WithMessage("Cost price must be positive when specified");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Quantity.HasValue)
            .WithMessage("Quantity cannot be negative");

        RuleFor(x => x.Sku)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Sku))
            .WithMessage("SKU cannot exceed 50 characters");

        RuleForEach(x => x.PaymentMethods)
            .Must(value => value >= 1 && value <= 5)
            .WithMessage(
                "PaymentMethod must be one of the following: 1 (PurchaseCard), 2 (CashOnDelivery), 3 (Fawry), 4 (Forsa), 5 (PayTabsAman).");

        RuleFor(x => x.ShippingType)
            .Must(x => Enum.IsDefined(typeof(ShippingType), x))
            .WithMessage("Invalid shipping type");

        RuleFor(x => x.ProductStatus)
            .Must(value => value == ProductStatus.Published || value == ProductStatus.Draft)
            .WithMessage("ProductStatus must be 1 (Published) or 2 (Draft).");

        RuleFor(x => x.ProductType)
            .Must(value => value == ProductTypes.Used || value == ProductTypes.New)
            .WithMessage("ProductStatus must be 1 (Used) or 2 (New).");

        RuleFor(x => x.Images)
            .Must(x => x.Count > 0)
            .WithMessage("At least one image is required")
            .Must(x => x.Count(i => i.IsMain) == 1)
            .WithMessage("Exactly one image must be marked as main");
    }
}