using FluentValidation;

namespace SnapSell.Application.Features.colors.Queries;

public sealed class GetAllColorsQueryValidator : AbstractValidator<GetAllColorsQuery>
{
    public GetAllColorsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}