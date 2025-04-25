using System.Text.Json.Serialization;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobIntentsionResponseDto
    {
        [JsonPropertyName("payment_keys")]
        public List<PaymentKeyDto> PaymentKeys { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("intention_detail")]
        public IntentionDetailDto IntentionDetail { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecretDto { get; set; }

        [JsonPropertyName("payment_methods")]
        public List<PaymentMethodDto> PaymentMethods { get; set; }

        [JsonPropertyName("special_reference")]
        public string SpecialReference { get; set; }

        [JsonPropertyName("extras")]
        public ExtrasDto Extras { get; set; }

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("card_detail")]
        public string CardDetail { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }
    public class PaymentKeyDto
    {
        [JsonPropertyName("integration")]
        public int Integration { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("gateway_type")]
        public string GatewayType { get; set; }

        [JsonPropertyName("iframe_id")]
        public string IframeId { get; set; }
    }
    public class IntentionDetailDto
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("items")]
        public List<ItemDto> Items { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class PaymentMethodDto
    {
        [JsonPropertyName("integration_id")]
        public int IntegrationId { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("method_type")]
        public string MethodType { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("live")]
        public bool Live { get; set; }
    }

    public class ExtrasDto
    {
        [JsonPropertyName("creation_extras")]
        public CreationExtrasDto CreationExtras { get; set; }

        [JsonPropertyName("confirmation_extras")]
        public string ConfirmationExtras { get; set; }
    }

    public class CreationExtrasDto
    {
        [JsonPropertyName("ee")]
        public int Ee { get; set; }
    }

    public class ItemDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

}
