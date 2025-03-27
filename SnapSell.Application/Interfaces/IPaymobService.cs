using SnapSell.Domain.Dtos.PaymobDtos;

namespace SnapSell.Application.Interfaces
{
    public interface IPaymobService
    {
        Task<PaymobIntentsionResponseDto> CreatePayment(PaymobCreatePaymentRequestDto model);
        bool IsAuthenticateCallback(PaymobCallbackRequestDto callbackResponseDto, string hmac);
        bool IsAuthenticateSaveCard(PaymobTokenCallbackResponseDto model, string hmac);
    }
}
