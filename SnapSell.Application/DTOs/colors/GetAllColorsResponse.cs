namespace SnapSell.Application.DTOs.colors;

public sealed record GetAllColorsResponse(
    Guid ColorId,
    string Name,
    string HexCode);