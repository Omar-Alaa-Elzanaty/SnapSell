using MediatR;
using SnapSell.Domain.Dtos.PaymobDtos;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnapSell.Application.Applications.Payment.Commnad.Callback
{
    public class PaymentCallbackCommand:IRequest<Result<int>>
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("obj")]
        public CallbackTransactionObjDto Obj { get; set; }

        [JsonPropertyName("issuer_bank")]
        public object IssuerBank { get; set; }

        [JsonPropertyName("transaction_processed_callback_responses")]
        public string TransactionProcessedCallbackResponses { get; set; }
    }

    public class CallbackTransactionObjDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("pending")]
        public bool Pending { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("success")]
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
        public long IntegrationId { get; set; }

        [JsonPropertyName("profile_id")]
        public long ProfileId { get; set; }

        [JsonPropertyName("has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        [JsonPropertyName("order")]
        public CallbackOrderDto Order { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("transaction_processed_callback_responses")]
        public List<object> TransactionProcessedCallbackResponses { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("source_data")]
        public SourceData SourceData { get; set; }

        [JsonPropertyName("api_source")]
        public string ApiSource { get; set; }

        [JsonPropertyName("terminal_id")]
        public object TerminalId { get; set; }

        [JsonPropertyName("merchant_commission")]
        public int MerchantCommission { get; set; }

        [JsonPropertyName("installment")]
        public object Installment { get; set; }

        [JsonPropertyName("discount_details")]
        public List<object> DiscountDetails { get; set; }

        [JsonPropertyName("is_void")]
        public bool IsVoid { get; set; }

        [JsonPropertyName("is_refund")]
        public bool IsRefund { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }

        [JsonPropertyName("payment_key_claims")]
        public PaymentKeyClaims PaymentKeyClaims { get; set; }

        [JsonPropertyName("error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonPropertyName("is_live")]
        public bool IsLive { get; set; }

        [JsonPropertyName("other_endpoint_reference")]
        public object OtherEndpointReference { get; set; }

        [JsonPropertyName("refunded_amount_cents")]
        public int RefundedAmountCents { get; set; }
    }

    public class CallbackOrderDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        [JsonPropertyName("merchant")]
        public MerchantDto Merchant { get; set; }

        [JsonPropertyName("collector")]
        public object Collector { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("shipping_data")]
        public ShippingDataDto ShippingData { get; set; }

        [JsonPropertyName("currency")]
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
        public object MerchantOrderId { get; set; }

        [JsonPropertyName("wallet_notification")]
        public object WalletNotification { get; set; }

        [JsonPropertyName("paid_amount_cents")]
        public int PaidAmountCents { get; set; }

        [JsonPropertyName("notify_user_with_email")]
        public bool NotifyUserWithEmail { get; set; }

        [JsonPropertyName("items")]
        public List<object> Items { get; set; }

        [JsonPropertyName("order_url")]
        public string OrderUrl { get; set; }

        [JsonPropertyName("commission_fees")]
        public int CommissionFees { get; set; }

        [JsonPropertyName("delivery_fees_cents")]
        public int DeliveryFeesCents { get; set; }

        [JsonPropertyName("delivery_vat_cents")]
        public int DeliveryVatCents { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("merchant_staff_tag")]
        public object MerchantStaffTag { get; set; }

        [JsonPropertyName("api_source")]
        public string ApiSource { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }

    public class MerchantDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("phones")]
        public List<string> Phones { get; set; }

        [JsonPropertyName("company_emails")]
        public List<string> CompanyEmails { get; set; }

        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }
    }

    public class ShippingDataDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("building")]
        public string Building { get; set; }

        [JsonPropertyName("floor")]
        public string Floor { get; set; }

        [JsonPropertyName("apartment")]
        public string Apartment { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("email")]
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
        public long OrderId { get; set; }
    }

    public class SourceData
    {
        [JsonPropertyName("pan")]
        public string Pan { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("tenure")]
        public object Tenure { get; set; }

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("gateway_integration_pk")]
        public long GatewayIntegrationPk { get; set; }

        [JsonPropertyName("klass")]
        public string Klass { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("migs_order")]
        public MigsOrderDto MigsOrder { get; set; }

        [JsonPropertyName("merchant")]
        public string Merchant { get; set; }

        [JsonPropertyName("migs_result")]
        public string MigsResult { get; set; }

        [JsonPropertyName("migs_transaction")]
        public MigsTransactionDto MigsTransaction { get; set; }

        [JsonPropertyName("txn_response_code")]
        public string TxnResponseCode { get; set; }

        [JsonPropertyName("acq_response_code")]
        public string AcqResponseCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("merchant_txn_ref")]
        public long MerchantTxnRef { get; set; }

        [JsonPropertyName("order_info")]
        public long OrderInfo { get; set; }

        [JsonPropertyName("receipt_no")]
        public string ReceiptNo { get; set; }

        [JsonPropertyName("transaction_no")]
        public string TransactionNo { get; set; }

        [JsonPropertyName("batch_no")]
        public long BatchNo { get; set; }

        [JsonPropertyName("authorize_id")]
        public string AuthorizeId { get; set; }

        [JsonPropertyName("card_type")]
        public string CardType { get; set; }

        [JsonPropertyName("card_num")]
        public string CardNum { get; set; }

        [JsonPropertyName("secure_hash")]
        public string SecureHash { get; set; }

        [JsonPropertyName("avs_result_code")]
        public string AvsResultCode { get; set; }

        [JsonPropertyName("avs_acq_response_code")]
        public string AvsAcqResponseCode { get; set; }

        [JsonPropertyName("captured_amount")]
        public float CapturedAmount { get; set; }

        [JsonPropertyName("authorised_amount")]
        public float AuthorisedAmount { get; set; }

        [JsonPropertyName("refunded_amount")]
        public float RefundedAmount { get; set; }

        [JsonPropertyName("acs_eci")]
        public string AcsEci { get; set; }
    }

    public class MigsOrderDto
    {
        [JsonPropertyName("acceptPartialAmount")]
        public bool AcceptPartialAmount { get; set; }

        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        [JsonPropertyName("authenticationStatus")]
        public string AuthenticationStatus { get; set; }

        [JsonPropertyName("chargeback")]
        public ChargebackDto Chargeback { get; set; }

        [JsonPropertyName("creationTime")]
        public string CreationTime { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [JsonPropertyName("merchantAmount")]
        public float MerchantAmount { get; set; }

        [JsonPropertyName("merchantCategoryCode")]
        public string MerchantCategoryCode { get; set; }

        [JsonPropertyName("merchantCurrency")]
        public string MerchantCurrency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("totalAuthorizedAmount")]
        public float TotalAuthorizedAmount { get; set; }

        [JsonPropertyName("totalCapturedAmount")]
        public float TotalCapturedAmount { get; set; }

        [JsonPropertyName("totalRefundedAmount")]
        public float TotalRefundedAmount { get; set; }
    }

    public class ChargebackDto
    {
        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }

    public class MigsTransactionDto
    {
        [JsonPropertyName("acquirer")]
        public AcquirerDto Acquirer { get; set; }

        [JsonPropertyName("amount")]
        public float Amount { get; set; }

        [JsonPropertyName("authenticationStatus")]
        public string AuthenticationStatus { get; set; }

        [JsonPropertyName("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("receipt")]
        public string Receipt { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("stan")]
        public string Stan { get; set; }

        [JsonPropertyName("terminal")]
        public string Terminal { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class AcquirerDto
    {
        [JsonPropertyName("batch")]
        public long Batch { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; }

        [JsonPropertyName("settlementDate")]
        public string SettlementDate { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }
    }

    public class PaymentKeyClaims
    {
        [JsonPropertyName("extra")]
        public object Extra { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }

        [JsonPropertyName("amount_cents")]
        public int AmountCents { get; set; }

        [JsonPropertyName("billing_data")]
        public BillingDataDto BillingData { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("integration_id")]
        public long IntegrationId { get; set; }

        [JsonPropertyName("lock_order_when_paid")]
        public bool LockOrderWhenPaid { get; set; }

        [JsonPropertyName("next_payment_intention")]
        public string NextPaymentIntention { get; set; }

        [JsonPropertyName("single_payment_attempt")]
        public bool SinglePaymentAttempt { get; set; }
    }

    public class BillingDataDto
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("floor")]
        public string Floor { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("building")]
        public string Building { get; set; }

        [JsonPropertyName("apartment")]
        public string Apartment { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("extra_description")]
        public string ExtraDescription { get; set; }
    }


}
