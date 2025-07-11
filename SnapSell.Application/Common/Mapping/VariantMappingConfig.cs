using Mapster;
using SnapSell.Application.Features.products.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.products.Commands.AddVariantsToProduct;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Models.SqlEntities;

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

        config.NewConfig<Variant, CreateProductVariantResponse>();

    }
}