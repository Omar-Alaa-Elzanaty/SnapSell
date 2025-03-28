using System.Text.Json.Serialization;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobRefundResponseDto
    {
        public int Id { get; set; }
        public bool Pending { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        public bool Success { get; set; }

        [JsonPropertyName("is_auth")]
        public bool IsAuth { get; set; }

        [JsonPropertyName("is_capture")]
        public bool IsCapture { get; set; }

        [JsonPropertyName("is_standalone_payment")]
        public bool IsStandalonePayment { get; set; }

        [JsonPropertyName("is_voided")]
        public bool IsVoided { get; set; }

        [JsonPropertyName("is_refunded")]
        public bool IsRefunded { get; set; }

        [JsonPropertyName("is_3d_secure")]
        public bool Is3DSecure { get; set; }

        [JsonPropertyName("integration_id")]
        public int IntegrationId { get; set; }

        [JsonPropertyName("profile_id")]
        public int ProfileId { get; set; }

        [JsonPropertyName("has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        public RefundOrderDto Order { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        public string Currency { get; set; }

        public PaymobSourceDataDto SourceData { get; set; }

        [JsonPropertyName("api_source")]
        public string ApiSource { get; set; }

        public bool IsVoid { get; set; }
        public bool IsRefund { get; set; }

        [JsonPropertyName("error_occured")]
        public bool ErrorOccurred { get; set; }

        [JsonPropertyName("refunded_amount_cents")]
        public object RefundedAmountCents { get; set; }

        public object CapturedAmount { get; set; }

        [JsonPropertyName("merchant_staff_tag")]
        public string MerchantStaffTag { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("is_settled")]
        public bool IsSettled { get; set; }

        [JsonPropertyName("bill_balanced")]
        public bool BillBalanced { get; set; }

        [JsonPropertyName("is_bill")]
        public bool IsBill { get; set; }

        public int Owner { get; set; }

        [JsonPropertyName("parent_transaction")]
        public int ParentTransaction { get; set; }
    }
    public class RefundOrderDto
    {
        public int Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        public PaymobRefundMerchantDto Merchant { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        public PaymobShippingDataDto ShippingData { get; set; }

        public string Currency { get; set; }

        [JsonPropertyName("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonPropertyName("is_return")]
        public bool IsReturn { get; set; }

        [JsonPropertyName("is_cancel")]
        public bool IsCancel { get; set; }

        [JsonPropertyName("is_returned")]
        public bool IsReturned { get; set; }

        [JsonPropertyName("is_canceled")]
        public bool IsCanceled { get; set; }

        [JsonPropertyName("merchant_order_id")]
        public string MerchantOrderId { get; set; }

        public bool? WalletNotification { get; set; }

        [JsonPropertyName("paid_amount_cents")]
        public int PaidAmountCents { get; set; }

        [JsonPropertyName("notify_user_with_email")]
        public bool NotifyUserWithEmail { get; set; }

        public List<PaymobItemDto> Items { get; set; }

        [JsonPropertyName("order_url")]
        public string OrderUrl { get; set; }

        public object CommissionFees { get; set; }

        [JsonPropertyName("delivery_fees_cents")]
        public object DeliveryFeesCents { get; set; }

        [JsonPropertyName("delivery_vat_cents")]
        public object DeliveryVatCents { get; set; }

        public string PaymentMethod { get; set; }

        [JsonPropertyName("merchant_staff_tag")]
        public string MerchantStaffTag { get; set; }

        [JsonPropertyName("api_source")]
        public string ApiSource { get; set; }

        public object Data { get; set; }
    }

    public class PaymobRefundMerchantDto
    {
        public int Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        public List<string> Phones { get; set; }
        public List<string> CompanyEmails { get; set; }

        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        public string Street { get; set; }
    }

    public class PaymobShippingDataDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("extra_description")]
        public string ExtraDescription { get; set; }

        [JsonPropertyName("shipping_method")]
        public string ShippingMethod { get; set; }

        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }

        public int Order { get; set; }
    }

    public class PaymobItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        public int Quantity { get; set; }
    }

    public class PaymobSourceDataDto
    {
        public string Type { get; set; }
        public string Pan { get; set; }

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; }

        public object Tenure { get; set; }
    }
}
