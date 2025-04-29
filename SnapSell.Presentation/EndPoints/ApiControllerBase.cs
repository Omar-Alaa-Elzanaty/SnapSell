using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;

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
        if (result.IsSuccess)
        {
            return result.StatusCode switch
            {
                HttpStatusCode.OK => Ok(result.Data),
                HttpStatusCode.Created => Created("", result.Data),
                _ => Ok(result.Data)
            };
        }

        return result.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(Result<bool>.Failure(message:"An Error happend",HttpStatusCode.BadRequest)),
            HttpStatusCode.NotFound => NotFound(Result<bool>.Failure(message: "An Error happend", HttpStatusCode.NotFound)),
            _ => StatusCode((int)result.StatusCode, new { result.Message, result.Errors })
        };
    }
}