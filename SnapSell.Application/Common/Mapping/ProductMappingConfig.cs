using Mapster;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Features.product.Queries.ProductSearch;
using SnapSell.Domain.Models.MongoDbEntities;
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
            //.Map(dest => dest.BrandName, src => src.Brand != null ? src.Brand.Name : null)
            //.Map(dest => dest.Variants, src => src.Variants);

        config.NewConfig<Product, CreateProductAdditionalInformationResponse>()
            .Map(dest => dest.ProductId, src => src.Id);

        config.NewConfig<Product, ProductSearchDto>()
            .Map(dest => dest.ProductId, src => src.Id);

        config.NewConfig<Variant, ProductVariantSearchResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SizeId, src => src.SizeId);
        
        config.NewConfig<Brand, BrandDto>()
            .Map(dest => dest.BrandId, src => src.Id);
        
        config.NewConfig<Category, CategoriesDto>()
            .Map(dest => dest.CategoryId, src => src.Id);
    }
}