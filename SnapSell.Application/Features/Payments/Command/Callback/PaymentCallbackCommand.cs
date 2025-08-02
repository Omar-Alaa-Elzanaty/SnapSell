using MediatR;
using Newtonsoft.Json;
using SnapSell.Domain.Dtos.PaymobDtos;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Payments.Command.Callback
{
    public class PaymentCallbackCommand:IRequest<Result<int>>
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("obj")]
        public CallbackTransactionObjDto Obj { get; set; }

        [JsonProperty("issuer_bank")]
        public object IssuerBank { get; set; }

        [JsonProperty("transaction_processed_callback_responses")]
        public string TransactionProcessedCallbackResponses { get; set; }
    }

    public class CallbackTransactionObjDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pending")]
        public bool Pending { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("is_auth")]
        public bool IsAuth { get; set; }

        [JsonProperty("is_capture")]
        public bool IsCapture { get; set; }

        [JsonProperty("is_standalone_payment")]
        public bool IsStandalonePayment { get; set; }

        [JsonProperty("is_voided")]
        public bool IsVoided { get; set; }

        [JsonProperty("is_refunded")]
        public bool IsRefunded { get; set; }

        [JsonProperty("is_3d_secure")]
        public bool Is3DSecure { get; set; }

        [JsonProperty("integration_id")]
        public long IntegrationId { get; set; }

        [JsonProperty("profile_id")]
        public long ProfileId { get; set; }

        [JsonProperty("has_parent_transaction")]
        public bool HasParentTransaction { get; set; }

        [JsonProperty("order")]
        public CallbackOrderDto Order { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("transaction_processed_callback_responses")]
        public List<object> TransactionProcessedCallbackResponses { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("source_data")]
        public SourceData SourceData { get; set; }

        [JsonProperty("api_source")]
        public string ApiSource { get; set; }

        [JsonProperty("terminal_id")]
        public object TerminalId { get; set; }

        [JsonProperty("merchant_commission")]
        public int MerchantCommission { get; set; }

        [JsonProperty("installment")]
        public object Installment { get; set; }

        [JsonProperty("discount_details")]
        public List<object> DiscountDetails { get; set; }

        [JsonProperty("is_void")]
        public bool IsVoid { get; set; }

        [JsonProperty("is_refund")]
        public bool IsRefund { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("payment_key_claims")]
        public PaymentKeyClaims PaymentKeyClaims { get; set; }

        [JsonProperty("error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonProperty("is_live")]
        public bool IsLive { get; set; }

        [JsonProperty("other_endpoint_reference")]
        public object OtherEndpointReference { get; set; }

        [JsonProperty("refunded_amount_cents")]
        public int RefundedAmountCents { get; set; }
    }

    public class CallbackOrderDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("delivery_needed")]
        public bool DeliveryNeeded { get; set; }

        [JsonProperty("merchant")]
        public MerchantDto Merchant { get; set; }

        [JsonProperty("collector")]
        public object Collector { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("shipping_data")]
        public ShippingDataDto ShippingData { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("is_payment_locked")]
        public bool IsPaymentLocked { get; set; }

        [JsonProperty("is_return")]
        public bool IsReturn { get; set; }

        [JsonProperty("is_cancel")]
        public bool IsCancel { get; set; }

        [JsonProperty("is_returned")]
        public bool IsReturned { get; set; }

        [JsonProperty("is_canceled")]
        public bool IsCanceled { get; set; }

        [JsonProperty("merchant_order_id")]
        public object MerchantOrderId { get; set; }

        [JsonProperty("wallet_notification")]
        public object WalletNotification { get; set; }

        [JsonProperty("paid_amount_cents")]
        public int PaidAmountCents { get; set; }

        [JsonProperty("notify_user_with_email")]
        public bool NotifyUserWithEmail { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }

        [JsonProperty("order_url")]
        public string OrderUrl { get; set; }

        [JsonProperty("commission_fees")]
        public int CommissionFees { get; set; }

        [JsonProperty("delivery_fees_cents")]
        public int DeliveryFeesCents { get; set; }

        [JsonProperty("delivery_vat_cents")]
        public int DeliveryVatCents { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("merchant_staff_tag")]
        public object MerchantStaffTag { get; set; }

        [JsonProperty("api_source")]
        public string ApiSource { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class MerchantDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("phones")]
        public List<string> Phones { get; set; }

        [JsonProperty("company_emails")]
        public List<string> CompanyEmails { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }
    }

    public class ShippingDataDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("floor")]
        public string Floor { get; set; }

        [JsonProperty("apartment")]
        public string Apartment { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("extra_description")]
        public string ExtraDescription { get; set; }

        [JsonProperty("shipping_method")]
        public string ShippingMethod { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }
    }

    public class SourceData
    {
        [JsonProperty("pan")]
        public string Pan { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tenure")]
        public object Tenure { get; set; }

        [JsonProperty("sub_type")]
        public string SubType { get; set; }
    }

    public class Data
    {
        [JsonProperty("gateway_integration_pk")]
        public long GatewayIntegrationPk { get; set; }

        [JsonProperty("klass")]
        public string Klass { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("migs_order")]
        public MigsOrderDto MigsOrder { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("migs_result")]
        public string MigsResult { get; set; }

        [JsonProperty("migs_transaction")]
        public MigsTransactionDto MigsTransaction { get; set; }

        [JsonProperty("txn_response_code")]
        public string TxnResponseCode { get; set; }

        [JsonProperty("acq_response_code")]
        public string AcqResponseCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("merchant_txn_ref")]
        public long MerchantTxnRef { get; set; }

        [JsonProperty("order_info")]
        public long OrderInfo { get; set; }

        [JsonProperty("receipt_no")]
        public string ReceiptNo { get; set; }

        [JsonProperty("transaction_no")]
        public string TransactionNo { get; set; }

        [JsonProperty("batch_no")]
        public long BatchNo { get; set; }

        [JsonProperty("authorize_id")]
        public string AuthorizeId { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_num")]
        public string CardNum { get; set; }

        [JsonProperty("secure_hash")]
        public string SecureHash { get; set; }

        [JsonProperty("avs_result_code")]
        public string AvsResultCode { get; set; }

        [JsonProperty("avs_acq_response_code")]
        public string AvsAcqResponseCode { get; set; }

        [JsonProperty("captured_amount")]
        public float CapturedAmount { get; set; }

        [JsonProperty("authorised_amount")]
        public float AuthorisedAmount { get; set; }

        [JsonProperty("refunded_amount")]
        public float RefundedAmount { get; set; }

        [JsonProperty("acs_eci")]
        public string AcsEci { get; set; }
    }

    public class MigsOrderDto
    {
        [JsonProperty("acceptPartialAmount")]
        public bool AcceptPartialAmount { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("authenticationStatus")]
        public string AuthenticationStatus { get; set; }

        [JsonProperty("chargeback")]
        public ChargebackDto Chargeback { get; set; }

        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [JsonProperty("merchantAmount")]
        public float MerchantAmount { get; set; }

        [JsonProperty("merchantCategoryCode")]
        public string MerchantCategoryCode { get; set; }

        [JsonProperty("merchantCurrency")]
        public string MerchantCurrency { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("totalAuthorizedAmount")]
        public float TotalAuthorizedAmount { get; set; }

        [JsonProperty("totalCapturedAmount")]
        public float TotalCapturedAmount { get; set; }

        [JsonProperty("totalRefundedAmount")]
        public float TotalRefundedAmount { get; set; }
    }

    public class ChargebackDto
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class MigsTransactionDto
    {
        [JsonProperty("acquirer")]
        public AcquirerDto Acquirer { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("authenticationStatus")]
        public string AuthenticationStatus { get; set; }

        [JsonProperty("authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("receipt")]
        public string Receipt { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("stan")]
        public string Stan { get; set; }

        [JsonProperty("terminal")]
        public string Terminal { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class AcquirerDto
    {
        [JsonProperty("batch")]
        public long Batch { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("settlementDate")]
        public string SettlementDate { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }

    public class PaymentKeyClaims
    {
        [JsonProperty("extra")]
        public object Extra { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("billing_data")]
        public BillingDataDto BillingData { get; set; }

        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonProperty("integration_id")]
        public long IntegrationId { get; set; }

        [JsonProperty("lock_order_when_paid")]
        public bool LockOrderWhenPaid { get; set; }

        [JsonProperty("next_payment_intention")]
        public string NextPaymentIntention { get; set; }

        [JsonProperty("single_payment_attempt")]
        public bool SinglePaymentAttempt { get; set; }
    }

    public class BillingDataDto
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("floor")]
        public string Floor { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("apartment")]
        public string Apartment { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("extra_description")]
        public string ExtraDescription { get; set; }
    }


}
