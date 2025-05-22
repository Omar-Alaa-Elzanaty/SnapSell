using Mapster;
using SnapSell.Application.Features.brands.Queries;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Common.Mapping;

public class BrandMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Brand, GetAllBrandsResponse>()
            .Map(dest => dest.BrandId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);
    }
}