using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.store.Commands.CreateStore;

public sealed record CreateStoreCommand(
    string Name,
    string Description,
    int MinimumDeliverPeriod,
    int MaximumDeliverPeriod,
    int DeliverPeriodTypes,
    MediaFileDto LogoUrl) : IRequest<Result<CreateStoreResponse>>;

public sealed record CreateStoreResponse
{
    public Guid Id { get; set; }
    public string SellerId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int MinimumDeliverPeriod { get; init; }
    public int MaximumDeliverPeriod { get; init; }
    public StoreStatusTypes Status { get; init; }
    public DeliverPeriodTypes DeliverPeriodTypes { get; init; }
    public string? LogoUrl { get; set; }
}