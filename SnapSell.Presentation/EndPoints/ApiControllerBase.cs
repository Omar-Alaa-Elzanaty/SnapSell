using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase(ICacheService cacheService) : ControllerBase
{
    protected async Task<ObjectResult> StatusCode<T>(Result<T> data)
    {
        data.CacheCodes = await cacheService.GetCacheCodes();

        return StatusCode((int)data.StatusCode, data);
    }

    protected async Task<ObjectResult> StatusCode<T>(PaginatedResult<T> data)
    {
        data.CacheCodes = await cacheService.GetCacheCodes();

        return StatusCode((int)data.StatusCode, data);
    }
    protected IActionResult HandleMediatorResult<TResult>(Result<TResult> result)
    {
        return StatusCode((int)result.StatusCode, result);
    }
}