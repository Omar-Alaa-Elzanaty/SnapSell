using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace SnapSell.Application.Features.product.Commands.UploadProductVideo;

public sealed class UploadProductVideoCommandValidator : AbstractValidator<UploadProductVideoCommand>
{
    private const long MaxVideoSize = 100 * 1024 * 1024; // 100MB
    private static readonly string[] AllowedVideoExtensions = [".mp4", ".mov", ".webm"];

    public UploadProductVideoCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Video)
            .NotNull().WithMessage("Video file is required.")
            .Must(file => file.Length > 0).WithMessage("Uploaded file is empty.")
            .Must(file => file.Length <= MaxVideoSize).WithMessage("Video file must not exceed 100MB.")
            .Must(BeAValidVideo).WithMessage("Invalid video format. Allowed formats: .mp4, .mov, .webm");
    }

    private bool BeAValidVideo(IFormFile file)
    {
        if (file == null) return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedVideoExtensions.Contains(extension) && file.ContentType.StartsWith("video/");
    }
}