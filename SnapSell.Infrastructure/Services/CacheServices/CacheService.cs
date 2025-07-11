using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.Interfaces;
using System.Security.Claims;
using SnapSell.Application.Abstractions.Interfaces;

namespace SnapSell.Infrastructure.Services.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CacheService(
            IHttpContextAccessor contextAccessor,
            IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<CacheResponse> GetCacheCodes()
        {
            var CacheCodes = new CacheResponse();

            if (bool.TryParse(_contextAccessor.HttpContext?.Request.Headers["GeneralCache"].ToString(), out bool isGeneralOn) && isGeneralOn)
            {
                var categoryCache = await _unitOfWork.CacheCodesRepo.Entities.FirstAsync(x => x.CacheKey == typeof(CategoryCache).Name);
                var brandCache = await _unitOfWork.CacheCodesRepo.Entities.FirstAsync(x => x.CacheKey == typeof(BrandCache).Name);

                CacheCodes.GeneralCache = new GeneralCahce
                {
                    CategoryCache = new CategoryCache
                    {
                        Version = categoryCache.Version,
                        LastUpdated = categoryCache.LastUpdated
                    },
                    BrandCache = new BrandCache
                    {
                        Version = brandCache.Version,
                        LastUpdated = brandCache.LastUpdated
                    }
                };
            }

            if (bool.TryParse(_contextAccessor.HttpContext?.Response.Headers["UserCache"].ToString(), out bool isUserOn) && isUserOn)
            {
                var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var profileCache = await _unitOfWork.CacheCodesRepo.Entities.FirstAsync(x => x.CacheKey == typeof(ProfileCache).Name && x.UserId == userId);
                CacheCodes.UserCache = new UserCache
                {
                    ProfileCache = new ProfileCache
                    {
                        Version = profileCache.Version,
                        LastUpdated = profileCache.LastUpdated
                    }
                };
            }

            return CacheCodes;
        }
        public async Task SetGeneralProperty<T>(string message) where T : IGeneralCache
        {
            var generalProperty = await _unitOfWork.CacheCodesRepo.Entities.FirstAsync(x => x.CacheKey == typeof(T).Name);
            generalProperty.Version++;
            generalProperty.LastUpdated = message;
        }

        public async Task SetUserProperty<T>(string message) where T : IUserCache
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userProperty = await _unitOfWork.CacheCodesRepo.Entities.FirstAsync(x => x.CacheKey == typeof(T).Name && x.UserId == userId);
            userProperty.Version++;
            userProperty.LastUpdated = message;
        }
    }
}