using Mapster;
using SnapSell.Application.DTOs.variant;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.colors.Queries;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Application.Features.product.Queries.GetAllPaymentMethods;
using SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Comman.mappingConfig;

public static class Mapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Brand, GetAllBrandsResponse>.NewConfig()
            .Map(dest => dest.BrandId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<Category, GetAllCategoriesResponse>.NewConfig()
            .Map(dest => dest.CategoryId, src => src.Id);

        TypeAdapterConfig<PaymentMethod, GetAllPaymentMethodsResponse>.NewConfig()
            .Map(dest => dest.PaymentMethodId, src => src.Id);


        TypeAdapterConfig<Color, GetAllColorsResponse>.NewConfig()
            .Map(dest => dest.ColorId, src => src.Id);

        TypeAdapterConfig<Product, CreateProductResponse>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id);

        TypeAdapterConfig<Variant, VariantResponse>.NewConfig()
            .Map(dest => dest.VariantId, src => src.Id);


        TypeAdapterConfig<Variant, VariantResponseInGetAllProductsToSeller>
            .NewConfig()
            .Map(dest => dest.VariantId, src => src.Id);

        TypeAdapterConfig<Product, GetAllProductsForSpecificSellerResponse>
            .NewConfig()
            .Map(dest => dest.ProductId, src => src.Id)

            .Map(dest => dest.BrandName, src => src.Brand != null ? src.Brand.Name : null)
            .Map(dest => dest.Variants, src => src.Variants);
    }
}