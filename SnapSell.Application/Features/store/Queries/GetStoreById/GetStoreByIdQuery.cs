using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

public sealed record GetStoreByIdQuery(Guid Id) : IRequest<Result<GetStoreByIdResponse>>;

public sealed record GetStoreByIdResponse
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int MinimumDeliverPeriod { get; init; }
    public int MaximumDeliverPeriod { get; init; }
    public string? LogoUrl { get; set; }
}