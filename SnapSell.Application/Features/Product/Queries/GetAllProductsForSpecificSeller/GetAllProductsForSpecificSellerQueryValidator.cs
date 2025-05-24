using FluentValidation;

namespace SnapSell.Application.Features.product.Queries.GetAllProductsForSpecificSeller;

public sealed class GetAllProductsForSpecificSellerQueryValidator
    : AbstractValidator<GetAllProductsForSpecificSellerQuery>
{
    public GetAllProductsForSpecificSellerQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty()
            .WithMessage("Seller ID is required")
            .MaximumLength(50)
            .WithMessage("Seller ID cannot exceed 50 characters");

        RuleFor(x => x.Pagination.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("Page number cannot exceed 1000");

        RuleFor(x => x.Pagination.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.Pagination.SortOrder)
            .Must(sortOrder => string.IsNullOrEmpty(sortOrder) ||
                               sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                               sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortOrder must be either 'asc' or 'desc' (case insensitive)");

    }
}