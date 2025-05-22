using Mapster;
using Microsoft.Extensions.Configuration;
using SnapSell.Application.Features.Payments.Commnad.Callback;
using SnapSell.Application.Features.Payments.Commnad.Token;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos.PaymobDtos;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SnapSell.Infrastructure.Services.PaymentGateway
{
    public class PaymobService : IPaymobService
    {
        private readonly IApiRequestHandleService _apiService;
        private readonly IConfiguration _config;

        public PaymobService(HttpClient httpClient,
            IApiRequestHandleService apiRequestHandleService,
            IConfiguration config)
        {

            _apiService = apiRequestHandleService;
            _apiService.HttpClient = httpClient;
            _config = config;
        }

        public bool IsAuthenticateCallback(PaymentCallbackCommand model, string hmac)
        {
            var plainText = model.Obj.AmountCents.ToString()
                          + model.Obj.CreatedAt.ToString()
                          + model.Obj.Currency.ToString()
                          + model.Obj.ErrorOccured.ToString()
                          + model.Obj.HasParentTransaction.ToString()
                          + model.Obj.Id.ToString()
                          + model.Obj.IntegrationId.ToString()
                          + model.Obj.Is3DSecure.ToString()
                          + model.Obj.IsAuth.ToString()
                          + model.Obj.IsCapture.ToString()
                          + model.Obj.IsRefund.ToString()
                          + model.Obj.IsStandalonePayment.ToString();


            return IsAuthenticatedCallback(plainText, hmac);
            
        }

        public async Task<PaymobIntentsionResponseDto> CreatePayment(PaymobIntenstionRequestDto model)
        {
            var paymentMethods = _config["Paymob:PaymentMethods"]!;

            model.PaymentMethods = paymentMethods.Split(',').ToList();
            model.RedirectUrl = _config["Paymob:FrontEndRedirectUrl"]!;
            model.NotificationUrl = _config["paymob:BackendCallbackMethod"]!;

            var serilizeObject = JsonSerializer.Serialize(model);

            var content = new StringContent(serilizeObject, new MediaTypeHeaderValue("application/json"));

            var response = await _apiService.SendAsync<PaymobIntentsionResponseDto>(_config["Paymob:IntentionUrl"]!, content);

            if (!response.IsSuccess)
            {
                throw new OperationCanceledException("Failed to create payment process.");
            }

            return response.Data!;
        }

        public async Task<PaymobSaveWithCardTokenResponseDto> PayWithSavedCardToken(string cardToken,string paymentKey)
        {
            var body = new
            {
                identifier = cardToken,
                payment_token = paymentKey
            };

            var serlize= JsonSerializer.Serialize(body);

            var content = new StringContent(serlize, new MediaTypeHeaderValue("application/json"));

            var response = await _apiService.SendAsync<PaymobSaveWithCardTokenResponseDto>(_config["Paymob:AcceptancePay"]!, content);

            if (!response.IsSuccess)
            {
                throw new OperationCanceledException("invalid operation");
            }

            return response.Data!;
        }

        public bool IsAuthenticateSaveCard(PaymentTokenCommand model, string hmac)
        {
            var plainText = model.Obj.CardSubType.ToString()
                          + model.Obj.CreatedAt.ToString()
                          + model.Obj.Email.ToString()
                          + model.Obj.Id.ToString()
                          + model.Obj.MaskedPan.ToString()
                          + model.Obj.merchantId.ToString()
                          + model.Obj.OrderId.ToString()
                          + model.Obj.Token.ToString();

            return IsAuthenticatedCallback(plainText, hmac);
        }

        public async Task<PaymobRefundResponseDto>RefundTransaction(string transactionId,string ammountInCents)
        {
            var body = new
            {
                transaction_id = transactionId,
                amount_cents = ammountInCents
            };

            var serlize = JsonSerializer.Serialize(body);

            var content = new StringContent(serlize, new MediaTypeHeaderValue("application/json"));

            var response = await _apiService.SendAsync<PaymobRefundResponseDto>(_config["Paymob:Refund"]!, content);

            if (!response.IsSuccess)
            {
                throw new OperationCanceledException("invalid operation");
            }

            return response.Data!;
        }

        private bool IsAuthenticatedCallback(string plainText, string key)
        {

            using HMACSHA512 hmac = new(Encoding.UTF8.GetBytes(key));

            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            var hashedtext = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            var hmcaKey = _config["HMAC"]!;

            return hashedtext == hmcaKey;
        }
    }
}
