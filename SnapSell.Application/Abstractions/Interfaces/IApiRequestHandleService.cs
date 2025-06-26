using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IApiRequestHandleService
{
    public HttpClient HttpClient { get; set; }
    Task<Result<T>> GetAsync<T>(string url);
    Task<Result<T>> SendAsync<T>(string url, HttpContent? httpContent);
}