using FluentValidation;

namespace SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;

public sealed class AddAdditionalInformationToProductCommandValidator
    : AbstractValidator<AddAdditionalInformationToProductCommand>
{
    public AddAdditionalInformationToProductCommandValidator()
    {
        RuleFor(x => x.EnglishDescription)
            .MaximumLength(200)
            .WithMessage("English description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.EnglishDescription));

        RuleFor(x => x.ArabicDescription)
            .MaximumLength(200)
            .WithMessage("Arabic description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.ArabicDescription));

        RuleFor(x => x.MinDeleveryDays)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Minimum delivery days must be at least 1")
            .LessThanOrEqualTo(x => x.MaxDeleveryDays)
            .WithMessage("Minimum delivery days must be ≤ maximum delivery days");

        RuleFor(x => x.MaxDeleveryDays)
            .GreaterThanOrEqualTo(x => x.MinDeleveryDays)
            .WithMessage("Maximum delivery days must be ≥ minimum delivery days")
            .LessThanOrEqualTo(30)
            .WithMessage("Maximum delivery days cannot exceed 30");

        RuleForEach(x => x.Variants)
            .SetValidator(new AddVariantsDtoValidator()!)
            .When(x => true);
    }
}

public sealed class AddVariantsDtoValidator : AbstractValidator<VariantDto>
{
    public AddVariantsDtoValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.RegularPrice)
            .NotEmpty()
            .WithMessage("Regular price must not be empty");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");

        RuleFor(x => x.SKU)
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.SKU));

        RuleFor(x => x.Barcode)
            .MaximumLength(50).WithMessage("Barcode must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Barcode));
    }
}