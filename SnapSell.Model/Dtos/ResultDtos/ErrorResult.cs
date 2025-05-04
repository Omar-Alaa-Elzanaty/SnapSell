using System.Net;

namespace SnapSell.Domain.Dtos.ResultDtos;

public class ErrorResult(
    string message,
    Dictionary<string, List<string>> errors,
    HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
{
    public string Message { get; set; } = message;
    public Dictionary<string, List<string>> Errors { get; set; } = errors;
    public int StatusCode { get; set; } = (int)statusCode;
}