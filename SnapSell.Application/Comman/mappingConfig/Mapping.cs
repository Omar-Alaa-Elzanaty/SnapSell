using Mapster;
using SnapSell.Application.DTOs.Brands;
using SnapSell.Application.DTOs.categories;
using SnapSell.Application.DTOs.colors;
using SnapSell.Application.DTOs.payment;
using SnapSell.Application.DTOs.Product;
using SnapSell.Application.DTOs.variant;
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
            .Map(dest => dest.CategoryId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.ParentCategoryId, src => src.ParentCategoryId);

        TypeAdapterConfig<PaymentMethod, GetAllPaymentMethodsResponse>.NewConfig()
            .Map(dest => dest.PaymentMethodId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);


        TypeAdapterConfig<Color, GetAllColorsResponse>.NewConfig()
            .Map(dest => dest.ColorId, src => src.Id);

        TypeAdapterConfig<Product, CreateProductResponse>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest.SellerId, src => src.CreatedBy)
            .Map(dest => dest.EnglishName, src => src.EnglishName)
            .Map(dest => dest.ArabicName, src => src.ArabicName)
            .Map(dest => dest.IsFeatured, src => src.IsFeatured)
            .Map(dest => dest.IsHidden, src => src.IsHidden);

        TypeAdapterConfig<Variant, VariantResponse>.NewConfig()
            .Map(dest => dest.VariantId, src => src.Id)
            .Map(dest => dest.SizeId, src => src.SizeId)
            .Map(dest => dest.ColorId, src => src.ColorId)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.RegularPrice, src => src.RegularPrice)
            .Map(dest => dest.SalePrice, src => src.SalePrice)
            .Map(dest => dest.SKU, src => src.SKU)
            .Map(dest => dest.Barcode, src => src.Barcode);


        TypeAdapterConfig<Product, GetAllProductsForSpecificSellerResponse>
            .NewConfig()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest.EnglishName, src => src.EnglishName)
            .Map(dest => dest.ArabicName, src => src.ArabicName)
            .Map(dest => dest.EnglishDescription, src => src.EnglishDescription)
            .Map(dest => dest.ArabicDescription, src => src.ArabicDescription)
            .Map(dest => dest.IsFeatured, src => src.IsFeatured)
            .Map(dest => dest.IsHidden, src => src.IsHidden)
            .Map(dest => dest.ProductStatus, src => src.ProductStatus)
            .Map(dest => dest.MinDeliveryDays, src => src.MinDeleveryDays)
            .Map(dest => dest.MaxDeliveryDays, src => src.MaxDeleveryDays)
            .Map(dest => dest.MainImageUrl, src => src.MainImageUrl)
            .Map(dest => dest.MainVideoUrl, src => src.MainVideoUrl)
            .Map(dest => dest.ShippingType, src => src.ShippingType)
            .Map(dest => dest.BrandName, src => src.Brand != null ? src.Brand.Name : string.Empty)
            .Map(dest => dest.VariantCount, src => src.Variants != null ? src.Variants.Count : 0);

    }
}