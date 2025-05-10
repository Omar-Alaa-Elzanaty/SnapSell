using MediatR;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.media;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.product.Commands.UploadProductImage;

public sealed record UploadProductImageCommand(Guid ProductId, IFormFile Image)
    : IRequest<Result<UploadProductImageResponse>>;