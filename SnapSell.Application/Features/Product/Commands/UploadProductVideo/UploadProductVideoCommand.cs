using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.UploadProductVideo;

public sealed record UploadProductVideoCommand(Guid ProductId,IFormFile Video) : IRequest<Result<UploeadProductVideoResponse>>;

public sealed record UploeadProductVideoResponse(string FullVideoUrl);

//public sealed record UploeadProductVideoResponse(
//    string FullVideoUrl,
//    string RelativePath,  // Optional if you want to return both
//    long FileSize,
//    string ContentType
//);