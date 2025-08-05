using FluentValidation;
using SnapSell.Domain.Enums;

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

        RuleFor(x => x.DeliverPeriodTypes)
            .Must(value => value == 1 || value == 2 || value == 3 || value == 4)
            .WithMessage("Valid values: 1 (Days), 2 (WorkingDays), 3 (Weeks), 4 (Months).");
    }
}