using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Authentication.Queries.LogIn;

public sealed record LogInQuery(string UserName, string Password) : IRequest<Result<LogInResult>>;
public sealed record LogInResult(LogInResponse User, string Token);

public sealed record LogInResponse(
    string UserId,
    string FirstName,
    string LastName,
    string UserName);