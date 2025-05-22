using Mapster;
using SnapSell.Application.Features.categories.Queries;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Common.Mapping;

internal class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, GetAllCategoriesResponse>()
            .Map(dest => dest.CategoryId, src => src.Id);
    }
}