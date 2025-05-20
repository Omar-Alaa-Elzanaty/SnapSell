using FluentValidation.Results;
using SnapSell.Domain.Extnesions;
using System.Net;

namespace SnapSell.Domain.Dtos.ResultDtos;

public sealed class PaginatedResult<T> : IResult<List<T>>
{
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    public HttpStatusCode StatusCode { get; set; }
    public List<T>? Data { get; set; }
    public string? Message { get; set; }
    public CacheResponse? CacheCodes { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }

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

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public PaginatedResult()
    {
    }

    public PaginatedResult(
        List<T> data,
        int count,
        int pageNumber,
        int pageSize,
        string? message = null,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Data = data;
        TotalCount = count;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Message = message;
        StatusCode = statusCode;
    }

    public PaginatedResult(
        string message,
        HttpStatusCode statusCode,
        Dictionary<string, List<string>>? errors = null)
    {
        Message = message;
        StatusCode = statusCode;
        Errors = errors;
        Data = new List<T>();
    }

    public static PaginatedResult<T> Success(
        List<T> data,
        int count,
        int pageNumber,
        int pageSize,
        string? message = null)
    {
        return new PaginatedResult<T>(data, count, pageNumber, pageSize, message);
    }

    public static Task<PaginatedResult<T>> SuccessAsync(
        List<T> data,
        int count,
        int pageNumber,
        int pageSize,
        string? message = null)
    {
        return Task.FromResult(Success(data, count, pageNumber, pageSize, message));
    }

    public static PaginatedResult<T> Failure(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        Dictionary<string, List<string>>? errors = null)
    {
        return new PaginatedResult<T>(message, statusCode, errors);
    }

    public static Task<PaginatedResult<T>> FailureAsync(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        Dictionary<string, List<string>>? errors = null)
    {
        return Task.FromResult(Failure(message, statusCode, errors));
    }

    public static PaginatedResult<T> ValidationFailure(
        List<ValidationFailure> validationFailures,
        string? message = null)
    {
        return new PaginatedResult<T>(
            message ?? "Validation failed",
            HttpStatusCode.UnprocessableEntity,
            validationFailures.GetErrorsDictionary());
    }
}