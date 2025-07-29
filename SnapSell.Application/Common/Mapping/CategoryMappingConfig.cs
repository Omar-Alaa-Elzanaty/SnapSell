using Mapster;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Application.Features.Products.Queries.ProductSearch;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Common.Mapping;

internal class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, GetAllCategoriesResponse>()
            .Map(dest => dest.CategoryId, src => src.Id);

        config.NewConfig<Category, CategoriesDto>()
            .Map(dest => dest.CategoryId, src => src.Id);
    }
}