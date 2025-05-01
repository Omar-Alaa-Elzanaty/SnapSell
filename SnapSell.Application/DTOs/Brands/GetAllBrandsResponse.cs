namespace SnapSell.Application.DTOs.Brands;

public sealed record GetAllBrandsResponse(Guid BrandId,string Name,string? LogoUrl);