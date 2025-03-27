using System.Text.Json.Serialization;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobIntenstionRequestDto
    {
        public double Amount { get; set; }
        [JsonPropertyName("payment_methods")]
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }
        [JsonPropertyName("billing_data")]
        public PaymobBillingDto BillingData { get; set; }
        [JsonPropertyName("redirection_url")]
        public string RedirectUrl { get; set; }
        [JsonPropertyName("notification_url")]
        public string NotificationUrl { get; set; }
    }

    public class PaymobBillingDto
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Country { get; set; }
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
