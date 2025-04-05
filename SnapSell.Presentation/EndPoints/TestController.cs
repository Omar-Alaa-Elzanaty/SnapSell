using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Presentation.EndPoints
{
    public class TestController:ApiControllerBase
    {
        private readonly IStringLocalizer<TestController> _localizer;

        public TestController(IStringLocalizer<TestController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var message = _localizer["Welcome"];
            return Ok(message.Value);
        }
    }
}
