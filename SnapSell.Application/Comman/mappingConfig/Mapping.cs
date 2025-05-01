using Mapster;
using SnapSell.Application.DTOs.Brands;
using SnapSell.Application.DTOs.categories;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Comman.mappingConfig;
public static class Mapping
{
    public static void Configure()
    {
        TypeAdapterConfig<Brand, GetAllBrandsResponse>.NewConfig()
            .Map(dest => dest.BrandId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LogoUrl, src => src.LogoUrl);

        TypeAdapterConfig<Category, GetAllCategoriesResponse>.NewConfig()
            .Map(dest => dest.CategoryId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.ParentCategoryId, src => src.ParentCategoryId);

    }
}