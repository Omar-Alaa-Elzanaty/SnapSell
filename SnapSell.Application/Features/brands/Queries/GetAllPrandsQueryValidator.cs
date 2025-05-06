using FluentValidation;

namespace SnapSell.Application.Features.brands.Queries;

public sealed class GetAllPrandsQueryValidator : AbstractValidator<GetAllPrandsQuery>
{
    public GetAllPrandsQueryValidator()
    {
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId is required.");
    }
}