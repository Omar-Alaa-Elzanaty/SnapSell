using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace SnapSell.Presentation.EndPoints
{
    public class TestController : ApiControllerBase
    {
        private readonly IStringLocalizer<TestController> _localizer;

        public TestController(IStringLocalizer<TestController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("Test")]
        public IActionResult GetABc()
        {
            //var message = _localizer["Welcome"];
            return Ok("");
        }
    }
}
