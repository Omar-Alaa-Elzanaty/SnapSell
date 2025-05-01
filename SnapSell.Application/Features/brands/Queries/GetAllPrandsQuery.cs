using MediatR;
using SnapSell.Application.DTOs.Brands;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.brands.Queries;

public sealed record GetAllPrandsQuery(string CurrentUserId) : IRequest<Result<List<GetAllBrandsResponse>>>;