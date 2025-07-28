using FluentValidation;

namespace SnapSell.Application.Features.Products.Commands.UpdateProductVariants;

public class UpdateProductVariantsCommandValidator
    : AbstractValidator<UpdateProductVariantsCommand>
{
    public UpdateProductVariantsCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID must have value.");
        
        RuleFor(x => x.Variants)
            .NotNull()
            .WithMessage("Variants list cannot be null");

        RuleFor(x => x.Variants)
            .NotEmpty()
            .When(x => x.HasVariants)
            .WithMessage("At least one variant is required when HasVariants=true");

        RuleFor(x => x.Variants)
            .Must(v => v.Count(dto => dto.IsDefault) == 1)
            .When(x => x.HasVariants && x.Variants.Any())
            .WithMessage("Exactly one variant must be marked as default");
    }
}