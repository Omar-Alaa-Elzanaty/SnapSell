using Mapster;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Domain.Models.MongoDbEntities;

namespace SnapSell.Application.Common.Mapping;

public sealed class ProductImageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductImage, ProductImageResponse>();
    }
}