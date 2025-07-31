using FluentValidation;

namespace SnapSell.Application.Features.Products.Commands.ClearProductVariants;

public sealed class ClearProductVariantsCommandValidator : AbstractValidator<ClearProductVariantsCommand>
{
    public ClearProductVariantsCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
    }
}