using Mapster;
using SnapSell.Application.Features.store.Commands.CreateStore;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Common.Mapping;

public class StoreMappingConfig:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Store, CreateStoreResponse>();
        
        config.NewConfig<CreateStoreCommand, Store>();
    }
}