using FluentValidation.Results;
using SnapSell.Domain.Extnesions;
using System.Net;

namespace SnapSell.Domain.Dtos.ResultDtos;

public sealed class PaginatedResult<TData> : IResult<PaginatedResponse<TData>>
{
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    public HttpStatusCode StatusCode { get; set; }
    public CacheResponse? CacheCodes { get; set; }
    public string? Message { get; set; }
    public PaginatedResponse<TData>? Data { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }

    public IResult ToValidationErrors(Dictionary<string, List<string>> errors, HttpStatusCode statusCode,
        string message)
    {
        return new PaginatedResult<TData>(message, statusCode, errors);
    }

    public PaginatedResult()
    {
        Data = new PaginatedResponse<TData>();
    }

    public PaginatedResult(
        List<TData> items,
        int totalCount,
        int pageNumber,
        int pageSize,
        string? message = null,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Data = new PaginatedResponse<TData>
        {
            Items = items,
            Meta = new PaginationMeta
            {
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
            }
        };
        StatusCode = statusCode;
        Message = message;
    }

    public PaginatedResult(
        string message,
        HttpStatusCode statusCode,
        Dictionary<string, List<string>>? errors = null)
    {
        Message = message;
        StatusCode = statusCode;
        Errors = errors;
        Data = new PaginatedResponse<TData>();
    }


    public static PaginatedResult<TData> Success(
        List<TData> data,
        int count,
        int pageNumber,
        int pageSize,
        string? message = null)
    {
        return new PaginatedResult<TData>(data, count, pageNumber, pageSize, message);
    }

    public static Task<PaginatedResult<TData>> SuccessAsync(
        List<TData> items,
        int totalCount,
        int pageNumber,
        int pageSize,
        string? message = null)
    {
        return Task.FromResult(new PaginatedResult<TData>(
            items: items,
            totalCount: totalCount,
            pageNumber: pageNumber,
            pageSize: pageSize,
            message: message));
    }

    public static PaginatedResult<TData> Failure(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        Dictionary<string, List<string>>? errors = null)
    {
        return new PaginatedResult<TData>(message, statusCode, errors);
    }

    public static Task<PaginatedResult<TData>> FailureAsync(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        Dictionary<string, List<string>>? errors = null)
    {
        return Task.FromResult(Failure(message, statusCode, errors));
    }

    public static PaginatedResult<TData> ValidationFailure(
        List<ValidationFailure> validationFailures,
        string? message = null)
    {
        return new PaginatedResult<TData>(
            message ?? "Validation failed",
            HttpStatusCode.UnprocessableEntity,
            validationFailures.GetErrorsDictionary());
    }
}

public class PaginatedResponse<TResult>
{
    public List<TResult> Items { get; set; } = new List<TResult>();
    public PaginationMeta Meta { get; set; } = new PaginationMeta();
}

public class PaginationMeta
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public int TotalPages
    {
        get
        {
            if (PageSize <= 0 || TotalCount <= 0)
                return 0;

            try
            {
                return (int)Math.Ceiling(TotalCount / (double)Math.Max(1, PageSize));
            }
            catch
            {
                return 0;
            }
        }
    }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}