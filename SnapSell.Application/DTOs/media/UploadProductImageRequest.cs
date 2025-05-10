using Microsoft.AspNetCore.Http;


namespace SnapSell.Application.DTOs.media;

public sealed record UploadProductImageRequest(Guid ProductId, IFormFile Image);

public sealed record UploadProductImageResponse(string? ImageUrl);