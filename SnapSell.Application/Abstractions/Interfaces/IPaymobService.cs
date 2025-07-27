using SnapSell.Application.Features.Payments.Command.Callback;
using SnapSell.Application.Features.Payments.Command.TokenCallback;
using SnapSell.Domain.Dtos.PaymobDtos;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IPaymobService
{
    Task<PaymobIntentsionResponseDto> CreatePayment(PaymobIntenstionRequestDto model);
    Task<PaymobRefundResponseDto> RefundTransaction(string transactionId, string ammountInCents);
    bool IsAuthenticateCallback(PaymentCallbackCommand callbackResponseDto, string hmac);
    bool IsAuthenticateSaveCard(PaymentTokenCommand model, string hmac);
    Task<PaymobSaveWithCardTokenResponseDto> CreatePaymentWithSavedCardToken(string cardToken, string paymentKey);
}