using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnapSell.Application.Features.Payments.Command.Callback;
using SnapSell.Application.Features.Payments.Command.TokenCallback;
using SnapSell.Domain.Dtos.PaymobDtos;
using System.Text.Json;

namespace SnapSell.Presentation.EndPoints
{
    public class PaymobController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PaymobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Callback")]
        public async Task<IActionResult> Callback([FromBody] JsonElement command, CancellationToken cancellationToken)
        {
            string type = command.GetProperty("type").GetString()!;

            var commandString = JsonConvert.SerializeObject(command);

            if (type == "TRANSACTION")
            {
                var paymentCallbackCommand = JsonConvert.DeserializeObject<PaymentCallbackCommand>(commandString);
                return Ok(await _mediator.Send(paymentCallbackCommand!, cancellationToken));
            }
            else if (type == "TOKEN")
            {
                var tokenCallbackCommand = JsonConvert.DeserializeObject<PaymentTokenCommand>(commandString);
                return Ok(await _mediator.Send(tokenCallbackCommand!, cancellationToken));
            }

            return BadRequest("Invalid command type. Expected 'TRANSACTION' or 'TOKEN.");
        }
    }
}
