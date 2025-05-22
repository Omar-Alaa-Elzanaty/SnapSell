using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Payments.Commnad.Callback;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SnapSell.Presentation.EndPoints;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase(ICacheService cacheService) : ControllerBase
{
    protected async Task<ActionResult<Result<TResult>>> HandleMediatorResult<TResult>(Result<TResult> result)
    {
        result.CacheCodes = await cacheService.GetCacheCodes();

        return StatusCode((int)result.StatusCode, result);
    }
    protected async Task<ActionResult<PaginatedResult<TResult>>> HandleMediatorResult<TResult>(PaginatedResult<TResult> result)
    {
        result.CacheCodes = await cacheService.GetCacheCodes();
        return StatusCode((int)result.StatusCode, result);
    }
}