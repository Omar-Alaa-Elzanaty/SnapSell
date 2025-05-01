using FluentValidation;

namespace SnapSell.Application.Features.brands.Queries;

public class GetAllPrandsQueryValidator : AbstractValidator<GetAllPrandsQuery>
{
    public GetAllPrandsQueryValidator()
    {
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("CurrentUserId is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("CurrentUserId must be a valid GUID.");
    }
}