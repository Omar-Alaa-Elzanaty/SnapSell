using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Interfaces
{
    public interface ICacheService
    {
        public Task<CacheResponse> GetCacheCodes();
        Task SetGeneralProperty<T>(string message) where T : IGeneralCache;
        Task SetUserProperty<T>(string message) where T : IUserCache;

    }
}
