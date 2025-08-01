using Mapster;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Features.Products.Queries.ProductSearch;
using SnapSell.Application.Features.Products.Queries.SearchForProduct;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, CreateProductResponse>()
            .Map(dest => dest.ProductId, src => src.Id);

        config.NewConfig<Product, GetAllProductsForSpecificSellerResponse>()
            .Map(dest => dest.ProductId, src => src.Id);


        config.NewConfig<Product, ProductSearchDto>()
            .Map(dest => dest.ProductId, src => src.Id);
        
        config.NewConfig<Product, SearchForProductQueryDto>()
            .Map(dest => dest.ImageUrl,
                src => src.Images.Where(i => i.IsMainImage).Select(i => i.ImageUrl).FirstOrDefault()!);
    }
}