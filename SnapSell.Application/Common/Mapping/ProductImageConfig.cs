using Mapster;
using SnapSell.Application.Features.Products.Queries.ProductSearch;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Common.Mapping;

public class ProductImageConfig:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductImage, ProductImageResponse>();
    }
}