using FluentValidation;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

public class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Description).NotEmpty()
            .WithMessage("Description is required");

        RuleFor(x => x.MinimumDeliverPeriod).NotEmpty()
            .WithMessage("MinimumDeliverPeriod is required");

        RuleFor(x => x.MaximumDeliverPeriod).NotEmpty()
            .WithMessage("MaximumDeliverPeriod is required");

        RuleFor(x => x.DeliverPeriodTypes).NotEmpty()
            .WithMessage("1 for (Days), 2 for (WorkingDays), 3 for (Weeks), 4 for (Months).");
    }
}