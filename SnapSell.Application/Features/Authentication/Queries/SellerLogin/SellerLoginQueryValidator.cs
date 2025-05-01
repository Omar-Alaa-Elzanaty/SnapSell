using FluentValidation;

namespace SnapSell.Application.Features.Authentication.Queries.SellerLogin;

public sealed class SellerLoginQueryValidator : AbstractValidator<SellerLoginQuery>
{
    public SellerLoginQueryValidator()
    {
        RuleFor(x => x.ShopName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
    }
}