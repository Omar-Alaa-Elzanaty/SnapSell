using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Password) : IRequest<Result<RegisterResult>>;
    
    
public class RegisterResult
{
    public RegisterUserInfo UserInfo { get; set; }
    public string Token { get; set; }
}

public class RegisterUserInfo
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
}