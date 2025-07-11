using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(string FullName,
    string UserName,
    string Password) : IRequest<Result<RegisterResult>>;
    
    
public sealed record RegisterResult(
    string UserId,
    string FullName,
    string UserName,
    string AccessToken);