using Mapster;
using SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Common.Mapping;

public class PaymentMethodMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentMethod, GetAllPaymentMethodsResponse>()
            .Map(dest => dest.PaymentMethodId, src => src.Id);
    }
}