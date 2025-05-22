using Mapster;
using SnapSell.Application.Features.colors.Queries;
using SnapSell.Domain.Models;

namespace SnapSell.Application.Common.Mapping;

public class ColorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Color, GetAllColorsResponse>()
            .Map(dest => dest.ColorId, src => src.Id);
    }
}