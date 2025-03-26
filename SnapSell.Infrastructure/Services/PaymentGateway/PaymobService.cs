using SnapSell.Application.Interfaces;

namespace SnapSell.Infrastructure.Services.PaymentGateway
{
    public class PaymobService : IPaymobService
    {
        private readonly IApiRequestHandleService _apiService;

        public PaymobService(HttpClient httpClient,
            IApiRequestHandleService apiRequestHandleService)
        {

            _apiService = apiRequestHandleService;
            _apiService.HttpClient = httpClient;
        }

        public Task<string> CreatePayment()
        {
            throw new NotImplementedException();
        }
    }
}
