using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        //protected ObjectResult StatusCode(Result<object> data)
        //{
        //    return StatusCode((int)data.StatusCode, data);
        //}
    }
}
