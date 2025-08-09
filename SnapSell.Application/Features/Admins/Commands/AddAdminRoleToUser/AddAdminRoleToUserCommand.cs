using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Admins.Commands.AddAdminRoleToUser;

public record AddAdminRoleToUserCommand(string UserId): IRequest<Result<AddAdminRoleToUserResponse>>;

public record AddAdminRoleToUserResponse(
    string Id,
    string UserName,
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName);

