using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {

        public ObjectResult StatusCode(Result<object> data)
        {
            return StatusCode((int)data.StatusCode, data);
        }
    }
}
