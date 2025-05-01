using FluentValidation;

namespace SnapSell.Application.Features.categories.Queries;

public sealed class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
{
    public GetAllCategoriesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("CurrentUserId is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("CurrentUserId must be a valid GUID.");
    }
}