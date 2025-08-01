using FluentValidation;

namespace SnapSell.Application.Features.Products.Commands.UpdateVariantById;

public sealed class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("VariantId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

        RuleFor(x => x.SizeId)
            .NotEmpty().WithMessage("SizeId is required.");
    }
}