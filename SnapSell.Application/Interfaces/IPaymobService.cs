﻿using SnapSell.Application.Applications.Payment.Commnad.Callback;
using SnapSell.Application.Applications.Payment.Commnad.Token;
using SnapSell.Domain.Dtos.PaymobDtos;

namespace SnapSell.Application.Interfaces
{
    public interface IPaymobService
    {
        Task<PaymobIntentsionResponseDto> CreatePayment(PaymobIntenstionRequestDto model);
        Task<PaymobRefundResponseDto> RefundTransaction(string transactionId, string ammountInCents);
        bool IsAuthenticateCallback(PaymentCallbackCommand callbackResponseDto, string hmac);
        bool IsAuthenticateSaveCard(PaymentTokenCommand model, string hmac);
        Task<PaymobSaveWithCardTokenResponseDto> PayWithSavedCardToken(string cardToken, string paymentKey);
    }
}
