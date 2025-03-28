using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnapSell.Domain.Dtos.PaymobDtos
{
    public class PaymobSaveWithCardTokenResponseDto
    {
        public int Id { get; set; }

        public string Pending { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        public string Success { get; set; }

        [JsonPropertyName("is_auth")]
        public string IsAuth { get; set; }

        [JsonPropertyName("is_capture")]
        public string IsCapture { get; set; }

        [JsonPropertyName("is_standalone_payment")]
        public string IsStandalonePayment { get; set; }

        public string IsVoided { get; set; }

        public string IsRefunded { get; set; }

        [JsonPropertyName("is_3d_secure")]
        public string Is3DSecure { get; set; }

        [JsonPropertyName("integration_id")]
        public int IntegrationId { get; set; }

        [JsonPropertyName("profile_id")]
        public int ProfileId { get; set; }

        [JsonPropertyName("has_parent_transaction")]
        public string HasParentTransaction { get; set; }

        public int Order { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        public string Currency { get; set; }

        public string TerminalId { get; set; }

        public object MerchantCommission { get; set; }

        public string Installment { get; set; }

        public object[] DiscountDetails { get; set; }

        public string IsVoid { get; set; }

        public string IsRefund { get; set; }

        [JsonPropertyName("error_occured")]
        public string ErrorOccurred { get; set; }

        public object RefundedAmountCents { get; set; }

        public object CapturedAmount { get; set; }

        public string MerchantStaffTag { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public string IsSettled { get; set; }

        public string BillBalanced { get; set; }

        public string IsBill { get; set; }

        public int Owner { get; set; }

        public string ParentTransaction { get; set; }

        [JsonPropertyName("merchant_order_id")]
        public string MerchantOrderId { get; set; }

        [JsonPropertyName("data.message")]
        public string DataMessage { get; set; }

        [JsonPropertyName("source_data.type")]
        public string SourceDataType { get; set; }

        [JsonPropertyName("source_data.pan")]
        public string SourceDataPan { get; set; }

        [JsonPropertyName("source_data.sub_type")]
        public string SourceDataSubType { get; set; }

        [JsonPropertyName("acq_response_code")]
        public string AcqResponseCode { get; set; }

        [JsonPropertyName("txn_response_code")]
        public string TxnResponseCode { get; set; }

        public string Hmac { get; set; }

        [JsonPropertyName("merchant_txn_ref")]
        public string MerchantTxnRef { get; set; }

        public bool UseRedirection { get; set; }

        public string RedirectionUrl { get; set; }

        public string MerchantResponse { get; set; }

        public bool BypassStepSix { get; set; }
    }
}
