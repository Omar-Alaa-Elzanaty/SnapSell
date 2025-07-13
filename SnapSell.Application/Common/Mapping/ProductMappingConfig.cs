using Mapster;
using SnapSell.Application.Features.products.Commands.AddAdditionalInformationToProduct;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Application.Features.products.Queries.GetAllProductsForSpecificSeller;
using SnapSell.Application.Features.Products.Queries.ProductSearch;
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
        
        config.NewConfig<Product, SearchResponse>()
            .Map(dest => dest.Product.ProductId, src => src.Id)
            .Map(dest => dest.Product.StoreId, src => src.StoreId)
            .Map(dest => dest.Product.EnglishName, src => src.EnglishName)
            .Map(dest => dest.Product.ArabicName, src => src.ArabicName)
            .Map(dest => dest.Product.EnglishDescription, src => src.EnglishDescription)
            .Map(dest => dest.Product.ArabicDescription, src => src.ArabicDescription)
            .Map(dest => dest.Product.IsFeatured, src => src.IsFeatured)
            .Map(dest => dest.Product.IsHidden, src => src.IsHidden)
            .Map(dest => dest.Product.ShippingType, src => src.ShippingType)
            .Map(dest => dest.Product.ProductStatus, src => src.ProductStatus)
            .Map(dest => dest.Product.MinDeliveryDays, src => src.MinDeliveryDays)
            .Map(dest => dest.Product.MaxDeliveryDays, src => src.MaxDeliveryDays)
            .Map(dest => dest.Product.Price, src => src.Price)
            .Map(dest => dest.Product.SalePrice, src => src.SalePrice)
            .Map(dest => dest.Product.CostPrice, src => src.CostPrice)
            .Map(dest => dest.Product.Quantity, src => src.Quantity)
            .Map(dest => dest.Product.Sku, src => src.Sku)
            .Map(dest => dest.Product.PaymentMethods, src => src.PaymentMethods)
            .Map(dest => dest.Product.Variants, src => src.Variants.Adapt<List<ProductVariantSearchResponse>>())
            .Map(dest => dest.Brand, src => src.Brand.Adapt<BrandDto>())
            .Map(dest => dest.Categorie, src => src.Category.Adapt<CategoriesDto>());

    }
}