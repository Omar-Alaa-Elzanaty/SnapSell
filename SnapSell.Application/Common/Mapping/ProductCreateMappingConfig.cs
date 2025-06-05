using Mapster;
using SnapSell.Application.Features.product.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, CreateProductResponse>()
            .Map(dest => dest.ProductId, src => src.Id);


        config.NewConfig<Product, GetAllProductsForSpecificSellerResponse>()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest.BrandName, src => src.Brand != null ? src.Brand.Name : null)
            .Map(dest => dest.Variants, src => src.Variants);

        config.NewConfig<Product, CreateProductAdditionalInformationResponse>()
            .Map(dest => dest.ProductId, src => src.Id);


    }
}