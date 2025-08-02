using MediatR;
using Newtonsoft.Json;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Payments.Command.TokenCallback
{
    public class PaymentTokenCommand : IRequest<Result<int>>
    {
        public string Type { get; set; }
        public ObjDto Obj { get; set; }
    }

    public class ObjDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        public string Token { get; set; }
        [JsonProperty("masked_pan")]
        public string MaskedPan { get; set; }
        [JsonProperty("merchant_id")]
        public string merchantId { get; set; }
        [JsonProperty("card_subtype")]
        public string CardSubType { get; set; }
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        public string Email { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
        [JsonProperty("user_added")]
        public bool UserAdded { get; set; }
        [JsonProperty("next_payment_intention")]
        public string NextPaymentIntention { get; set; }
    }
}
