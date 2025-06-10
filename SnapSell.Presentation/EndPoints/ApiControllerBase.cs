using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected async Task<ActionResult<Result<TResult>>> HandleMediatorResult<TResult>(Result<TResult> result)
    {
        return StatusCode((int)result.StatusCode, result);
    }

    protected async Task<ActionResult<PaginatedResult<TResult>>> HandleMediatorResult<TResult>(
        PaginatedResult<TResult> result)
    {
        return StatusCode((int)result.StatusCode, result);
    }
}