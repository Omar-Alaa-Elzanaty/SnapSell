using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IStringLocalizer<CreateProductCommandValidator> stringLocalizer)
    {
        RuleFor(c => c.BrandId)
            .NotEmpty().WithMessage("BrandId is required");

        RuleFor(c => c.CategoryIds)
            .NotEmpty().WithMessage("At least one category is required.")
            .Must(ids => ids.Count <= 3).WithMessage("A maximum of 3 categories is allowed.");

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage("English name is required");

        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage("Arabic name is required");
        RuleFor(x => x.ShippingType)
            .Must(value => value == 1 || value == 2)
            .WithMessage("ShippingType must be 1 (Paid) or 2 (Free).");

        RuleFor(x => x.ProductStatus)
            .Must(value => value == ProductStatus.Published || value == ProductStatus.Draft)
            .WithMessage("ProductStatus must be 1 (Published) or 2 (Draft).");

        RuleFor(x => x.ProductType)
            .Must(value => value == ProductTypes.Used || value == ProductTypes.New)
            .WithMessage("ProductStatus must be 1 (Used) or 2 (New).");

        RuleForEach(x => x.PaymentMethods)
            .Must(value => value >= 1 && value <= 5)
            .WithMessage(
                "PaymentMethod must be one of the following: 1 (PurchaseCard), 2 (CashOnDelivery), 3 (Fawry), 4 (Forsa), 5 (PayTabsAman).");

        RuleFor(x => x.MinDeliveryDays)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum delivery days must be 0 or more");

        RuleFor(x => x.MaxDeliveryDays)
            .GreaterThanOrEqualTo(x => x.MinDeliveryDays)
            .When(x => x.MaxDeliveryDays >= 0)
            .WithMessage("Maximum delivery days must be greater than or equal to minimum delivery days");

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one image is required")
            .Must(images => images.Count <= 10).WithMessage("Maximum 10 images allowed")
            .Must(images => images.Count(img => img.IsMain) == 1)
            .WithMessage("Exactly one image must be marked as main.");

        When(x => x.HasVariants == false, () =>
        {
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required for non-variant products")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.SalePrice)
                .LessThan(x => x.Price)
                .When(x => x.SalePrice > x.Price)
                .WithMessage("Sale price must be less than price");

            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Quantity is required for non-variant products")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or more");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("SKU is required for non-variant products")
                .MaximumLength(50).WithMessage("SKU must not exceed 50 characters");

            RuleFor(x => x.CostPrice)
                .NotNull().WithMessage("CoastPrice is required for non-variant products")
                .GreaterThanOrEqualTo(0).WithMessage("CoastPrice must be 0 or more");

            RuleFor(x => x.Variants)
                .NotNull().WithMessage("Variants must not be null when HasVariants is false")
                .Must(v => !v.Any())
                .WithMessage("Variants must be an empty list when HasVariants is false");
        });

        When(x => x.HasVariants, () =>
        {
            RuleFor(x => x.Variants)
                .Null().When(x => x.HasVariants == false)
                .NotNull().WithMessage("Variants are required for variant products")
                .NotEmpty().WithMessage("At least one variant is required for variant products")
                .Must(variants => variants.Count <= 50).WithMessage("Maximum 50 variants allowed")
                .Must(variants => variants.Count(v => v?.IsDefault == true) == 1)
                .WithMessage("Exactly one variant must be marked as default.");

            RuleFor(x => x.Price)
                .Null().WithMessage("Price must be null for variant products");

            RuleFor(x => x.SalePrice)
                .Null().WithMessage("SalePrice must be null for variant products");

            RuleFor(x => x.CostPrice)
                .Null().WithMessage("CoastPrice must be null for variant products");

            RuleFor(x => x.Quantity)
                .Null().WithMessage("Quantity must be null for variant products");

            RuleFor(x => x.Sku)
                .Null().WithMessage("SKU must be null for variant products");

            RuleForEach(x => x.Variants)
                .NotNull().WithMessage("Variant cannot be null")
                .SetValidator(new CreateProductVariantDtoValidator()!);
        });
    }
}