using SnapSell.Domain.Dtos.PaymobDtos;

namespace SnapSell.Application.Interfaces
{
    public interface IPaymobService
    {
        Task<PaymobIntentsionResponseDto> CreatePayment(PaymobCreatePaymentRequestDto model);
        Task Callback(PaymobCallbackRequestDto callbackResponseDto,string hmac);
        Task SaveCard(PaymobTokenCallbackResponseDto model,string hmac);
    }
}
