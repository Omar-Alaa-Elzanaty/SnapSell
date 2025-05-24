using MediatR;
using SnapSell.Domain.Dtos.PaymobDtos;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Payments.Command.Token
{
    public class PaymentTokenCommand:IRequest<Result<int>>
    {
        public string Type { get; set; }
        public ObjDto Obj { get; set; }
    }

    public class ObjDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        [JsonPropertyName("masked_pan")]
        public string MaskedPan { get; set; }
        [JsonPropertyName("merchant_id")]
        public string merchantId { get; set; }
        [JsonPropertyName("card_subtype")]
        public string CardSubType { get; set; }
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }
        [JsonPropertyName("user_added")]
        public bool UserAdded { get; set; }
        [JsonPropertyName("next_payment_intention")]
        public string NextPaymentIntention { get; set; }
    }
}
