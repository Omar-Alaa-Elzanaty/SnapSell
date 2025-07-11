using System.Net;

namespace SnapSell.Domain.Dtos.ResultDtos;

public interface IResult
{
    bool IsSuccess { get; }
    HttpStatusCode StatusCode { get; }
    string? Message { get; }
    Dictionary<string, List<string>>? Errors { get; }
    IResult ToValidationErrors(Dictionary<string, List<string>> errors, HttpStatusCode statusCode, string message);
}

public interface IResult<out TValue> : IResult
{
    TValue? Data { get; }
}