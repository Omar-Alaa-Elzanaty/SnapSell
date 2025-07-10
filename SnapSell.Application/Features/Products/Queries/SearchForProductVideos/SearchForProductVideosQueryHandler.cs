using FluentValidation;
using MediatR;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Extensions.Services;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.SearchForProductVideos
{
    internal class SearchForProductVideosQueryHandler : IRequestHandler<SearchForProductVideosQuery, PaginatedResult<SearchForProductVideosQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SearchForProductVideosQuery> _validator;

        public SearchForProductVideosQueryHandler(IUnitOfWork unitOfWork, IValidator<SearchForProductVideosQuery> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<PaginatedResult<SearchForProductVideosQueryDto>> Handle(SearchForProductVideosQuery query, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
            {
                return PaginatedResult<SearchForProductVideosQueryDto>.ValidationFailure(validationResult.Errors);
            }

            var filterBuilder = Builders<Product>.Filter;

            var filters = FilterDefinition<Product>.Empty;

            SortDefinition<Product> sort = null!;

            if (!query.CategoriesIds.IsEmptyOrNull())
            {
                filters = Builders<Product>.Filter.AnyIn(x => x.CategoryIds, query.CategoriesIds);
            }

            if (!query.BrandsIds.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.In(x => x.BrandId, query.BrandsIds);
            }

            if (!query.Colors.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.ElemMatch(x => x.Variants, Builders<Variant>.Filter.In(v => v.Color, query.Colors));
            }

            if (!query.SizesIds.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.ElemMatch(x => x.Variants, Builders<Variant>.Filter.In(v => v.SizeId, query.SizesIds));
            }

            filters &= Builders<Product>.Filter.Gte(x => x.SalePrice, query.MinPrice);
            filters &= Builders<Product>.Filter.Lte(x => x.SalePrice, query.MaxPrice);
            filters &= Builders<Product>.Filter.Where(x => x.MainVideoUrl != null && x.MainVideoUrl.Length > 0);

            var sortBuilder = Builders<Product>.Sort;
            switch (query.Filter)
            {
                case SearchForProductVideosFilters.Relevance:
                    break;
                case SearchForProductVideosFilters.Newest:
                    sort = sortBuilder.Descending(x => x.CreatedAt);
                    break;
                case SearchForProductVideosFilters.LowToHighPrice:
                    sort = sortBuilder.Ascending(x => x.SalePrice);
                    break;
                case SearchForProductVideosFilters.HighToLowPrice:
                    sort = sortBuilder.Ascending(x => x.SalePrice);
                    break;
            }

            var projection = Builders<Product>.Projection.Expression(p =>
            new SearchForProductVideosQueryDto
            {
                ProductId = p.Id,
                VideoUrl = p.MainVideoUrl!,
                Price = p.Price,
                SalePrice = p.SalePrice
            });

            var products = await _unitOfWork.ProductsRepo.Collection
                .ToPaginatedListAsync(
                filter: filters,
                sort: sort,
                projection: projection,
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
                cancellationToken: cancellationToken);

            return products;
        }
    }
}
