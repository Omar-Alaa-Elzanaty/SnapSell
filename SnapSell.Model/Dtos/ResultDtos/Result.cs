using FluentValidation.Results;
using SnapSell.Domain.Extnesions;
using System.Net;
using System.Text.Json.Serialization;

namespace SnapSell.Domain.Dtos.ResultDtos;

public class Result<T> : IResult<T>
{
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }

    [JsonPropertyName("statusCode")]
    public int StatusCodeNumber => (int)StatusCode;
    public T? Data { get; set; }
    public string? Message { get; set; }
    public CacheResponse? CacheCodes { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }

    public Result()
    {
    }

    public static Result<T> Success()
    {
        return new()
        {
            StatusCode = HttpStatusCode.OK
        };
    }

    public static Result<T> Success(T data, string? message = null)
    {
        return new()
        {
            Data = data,
            Message = message,
            StatusCode = HttpStatusCode.OK
        };
    }

    public static Result<T> Success(string message)
    {
        return new()
        {
            StatusCode = HttpStatusCode.OK,
            Message = message
        };
    }

    public static Result<T> Success(T data, string message, HttpStatusCode statusCode)
    {
        return new()
        {
            Data = data,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static Result<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new()
        {
            Message = message,
            StatusCode = statusCode
        };
    }

    public static Result<T> ValidationFailure(List<ValidationFailure> errors)
    {
        return new()
        {
            Errors = errors.GetErrorsDictionary(),
            StatusCode = HttpStatusCode.UnprocessableEntity
        };
    }

    public static Result<T> ValidationFailure(List<ValidationFailure> errors, string message)
    {
        return new()
        {
            Errors = errors.GetErrorsDictionary(),
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = message
        };
    }

    public IResult ToValidationErrors(Dictionary<string, List<string>> errors, HttpStatusCode statusCode,
        string message)
    {
        return new Result<T>
        {
            Errors = errors,
            StatusCode = statusCode,
            Message = message
        };
    }
}