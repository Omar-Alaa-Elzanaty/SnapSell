using MediatR;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Extensions.Services;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Features.Products.Queries.SearchForProduct;

internal sealed class SearchForProductQueryHandler(IUnitOfWork unitOfWork, IMediaService mediaService)
    : IRequestHandler<SearchForProductQuery, PaginatedResult<SearchForProductQueryDto>>
{
    public async Task<PaginatedResult<SearchForProductQueryDto>> Handle(SearchForProductQuery command,
        CancellationToken cancellationToken)
    {
        var entities = unitOfWork.ProductsRepo.Entities;

        if (!command.CategoriesIds.IsEmptyOrNull())
        {
            entities = entities.Where(x =>
                x.Categories.Select(productCategory => productCategory.CategoryId)
                    .Any(c => command.CategoriesIds!.Contains(c)));
        }

        if (!command.BrandsIds.IsEmptyOrNull())
        {
            entities = entities.Where(x => command.BrandsIds!.Contains(x.BrandId));
        }

        if (!command.Colors.IsEmptyOrNull())
        {
            entities = entities.Where(x => x.Variants.Any(v => command.Colors!.Contains(v.Color)));
        }

        if (!command.SizesIds.IsEmptyOrNull())
        {
            entities = entities.Where(x => x.Variants.Any(v => command.SizesIds!.Contains(v.SizeId)));
        }

        entities = entities.Where(x => x.SalePrice >= command.MinPrice);
        entities = entities.Where(x => x.SalePrice <= command.MaxPrice);
        switch (command.Filter)
        {
            case SearchForProductSorts.Relevance:
                break;
            case SearchForProductSorts.Newest:
                entities = entities.OrderByDescending(x => x.CreatedAt);
                break;
            case SearchForProductSorts.LowToHighPrice:
                entities = entities.OrderBy(x => x.SalePrice);
                break;
            case SearchForProductSorts.HighToLowPrice:
                entities = entities.OrderByDescending(x => x.SalePrice);
                break;
        }

        var projectedQuery = entities.Select(p => new SearchForProductQueryDto
        {
            Id = p.Id,
            ImageUrl = mediaService.GetUrl(p.Images.FirstOrDefault(i => i.IsMainImage)!.ImageUrl, MediaTypes.Image)!,
            Price = p.Price,
            SalePrice = p.SalePrice
        });

        return await projectedQuery
            .ToPaginatedListAsync(
                pageNumber: command.PageNumber,
                pageSize: command.PageSize,
                cancellationToken: cancellationToken);
    }
}