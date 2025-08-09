using FluentValidation;

namespace SnapSell.Application.Features.Admins.Commands.AddAdminRoleToUser;


public class AddAdminRoleToUserCommandValidator : AbstractValidator<AddAdminRoleToUserCommand>
{
    public AddAdminRoleToUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}