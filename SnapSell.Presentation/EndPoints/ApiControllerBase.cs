using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly ICacheService _cacheService;

        protected ApiControllerBase(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        protected async Task<ObjectResult> StatusCode<T>(Result<T> data)
        {
            data.CacheCodes = await _cacheService.GetCacheCodes();

            return StatusCode((int)data.StatusCode, data);
        }

        protected async Task<ObjectResult> StatusCode<T>(PaginatedResult<T> data)
        {
            data.CacheCodes = await _cacheService.GetCacheCodes();

            return StatusCode((int)data.StatusCode, data);
        }
    }
}