using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace SnapSell.Application.Features.product.Commands.UploadProductImage;

public sealed class UploadProductImageCommandValidator : AbstractValidator<UploadProductImageCommand>
{
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedMimeTypes = ["image/jpeg", "image/png", "image/gif", "image/webp"];

    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage("Image file is required")
            .Must(BeAValidSize)
            .WithMessage($"Maximum file size exceeded ({MaxFileSize / (1024 * 1024)}MB)")
            .Must(HaveValidExtension)
            .WithMessage($"Allowed extensions: {string.Join(", ", AllowedExtensions)}")
            .Must(HaveValidMimeType)
            .WithMessage("Invalid file type")
            .Must(BeAnImage)
            .WithMessage("Uploaded file must be an image");
    }

    private bool BeAValidSize(IFormFile file) => file.Length <= MaxFileSize;
    private bool HaveValidMimeType(IFormFile file) => AllowedMimeTypes.Contains(file.ContentType.ToLower());
    private bool BeAnImage(IFormFile file) => file.ContentType.StartsWith("image/");

    private bool HaveValidExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }
}