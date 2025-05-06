using FluentValidation;
using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.variant;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed class CreatProductCommandValidator : AbstractValidator<CreatProductCommand>
{
    public CreatProductCommandValidator()
    {
        RuleFor(c => c.BrandId).NotEmpty().WithMessage("BrandId is required");

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage("English name is required")
            .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage("Arabic name is required")
            .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(250).WithMessage("Short description must not exceed 250 characters");

        RuleFor(x => x.MinDeleveryDays)
            .GreaterThanOrEqualTo(1).WithMessage("Minimum delivery days must be at least 1")
            .LessThanOrEqualTo(x => x.MaxDeleveryDays)
            .WithMessage("Minimum delivery days must be less than or equal to maximum delivery days");

        RuleFor(x => x.MaxDeleveryDays)
            .GreaterThanOrEqualTo(x => x.MinDeleveryDays)
            .WithMessage("Maximum delivery days must be greater than or equal to minimum delivery days")
            .LessThanOrEqualTo(30).WithMessage("Maximum delivery days cannot exceed 30");

        RuleFor(x => x.MainImageUrl)
            .Must(BeAValidImage).WithMessage("Invalid image file");


        RuleFor(x => x.Video)
            .Must(BeAValidVideo).WithMessage("Invalid image file");
    }

    private bool BeAValidImage(IFormFile? file)
    {
        if (file is null) return false;

        var maxSize = 5 * 1024 * 1024; // 5Mb

        if (file.Length > maxSize)
        {
            throw new ArgumentException("File too large. Max size: 5MB.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension) &&
               file.ContentType.StartsWith("image/");
    }

    private bool BeAValidVideo(IFormFile? file)
    {
        if (file is null) return false;

        var maxSize = 50 * 1024 * 1024;  // 50Mb

        if (file.Length > maxSize)
        {
            throw new ArgumentException("File too large. Max size: 50MB.");
        }

        var allowedExtensions = new[] { ".mp4", ".mov", ".webm" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension) &&
               file.ContentType.StartsWith("video/");
    }

    public sealed class AddVariantsDtoValidator : AbstractValidator<VariantDto>
    {
        public AddVariantsDtoValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.RegularPrice)
                .GreaterThanOrEqualTo(x => x.Price)
                .WithMessage("Regular price must be greater than or equal to sale price");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");

            RuleFor(x => x.SKU)
                .MaximumLength(50).WithMessage("SKU must not exceed 50 characters");

            RuleFor(x => x.Barcode)
                .MaximumLength(50).WithMessage("Barcode must not exceed 50 characters");
        }
    }
}