using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Address.Commands.AddAddress
{
    public sealed record AddAddressCommand(
        string ClinetId,
        string fullName,
        string phoneNumber,
        string streetName,
        string buildingDetails,
        string country,
        string governorate,
        string city,
        string district,
        string? landmark,
        bool isDefault,
        double Longitude,
        double Latitude) : IRequest<Result<AddAddressResponse>>;
    public sealed record AddAddressResponse();
    
}
