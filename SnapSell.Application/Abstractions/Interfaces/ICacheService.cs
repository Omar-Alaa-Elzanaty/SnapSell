using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.Interfaces;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface ICacheService
{
    public Task<CacheResponse> GetCacheCodes();
    Task SetGeneralProperty<T>(string message) where T : IGeneralCache;
    Task SetUserProperty<T>(string message) where T : IUserCache;

}