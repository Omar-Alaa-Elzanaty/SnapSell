using Mapster;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.AddVariantsToProduct;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Application.Common.Mapping;

public class VariantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Variant, VariantResponse>()
            .Map(dest => dest.VariantId, src => src.Id);

        config.NewConfig<Variant, VariantsInAddVariantsToProductResponse>()
            .Map(dest => dest.VariantId, src => src.Id);

        config.NewConfig<Variant, VariantResponseInGetAllProductsToSeller>()
            .Map(dest => dest.VariantId, src => src.Id);

    }
}