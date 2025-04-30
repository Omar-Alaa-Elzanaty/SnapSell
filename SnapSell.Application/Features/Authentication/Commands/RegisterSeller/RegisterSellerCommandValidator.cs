using FluentValidation;

namespace SnapSell.Application.Features.Authentication.Commands.RegisterSeller;

public sealed class RegisterSellerCommandValidator:AbstractValidator<RegisterSellerCommand>
{
    public RegisterSellerCommandValidator()
    {
        RuleFor(x => x.SellerName)
            .NotEmpty().WithMessage("Seller name is required.")
            .MaximumLength(100);

        RuleFor(x => x.ShopName)
            .NotEmpty().WithMessage("Shop name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}