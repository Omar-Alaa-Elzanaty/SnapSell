using Microsoft.AspNetCore.Mvc;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Route("api/[controller]/[Action]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Task<ActionResult<Result<TResult>>> HandleMediatorResult<TResult>(Result<TResult> result)
    {
        return Task.FromResult<ActionResult<Result<TResult>>>(StatusCode((int)result.StatusCode, result));
    }

    protected Task<ActionResult<PaginatedResult<TResult>>> HandleMediatorResult<TResult>(
        PaginatedResult<TResult> result)
    {
        return Task.FromResult<ActionResult<PaginatedResult<TResult>>>(StatusCode((int)result.StatusCode, result));
    }
}