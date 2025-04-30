using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace SnapSell.Application.Features.product.Commands.CreateProduct;

public sealed class CreatProductCommandValidator:AbstractValidator<CreatProductCommand>
{
    public CreatProductCommandValidator()
    {
        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage("English name is required.")
            .MaximumLength(200);

        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage("Arabic name is required.")
            .MaximumLength(200);

        RuleFor(x => x.MinDeleveryDays)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.MaxDeleveryDays)
            .GreaterThanOrEqualTo(x => x.MinDeleveryDays)
            .WithMessage("Max delivery days must be greater than or equal to min delivery days.");

        RuleFor(x => x.MainImageUrl)
            .NotNull().WithMessage("Main image is required.")
            .Must(BeAValidImage).WithMessage("Only JPEG and PNG images are allowed.");
    }

    private static bool BeAValidImage(IFormFile file)
    {
        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return file.Length > 0 && permittedExtensions.Contains(extension);
    }
}