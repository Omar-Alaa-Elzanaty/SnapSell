using FluentValidation;
using Mapster;
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

            var entities = _unitOfWork.ProductsRepo.Entities;


            if (!query.CategoriesIds.IsEmptyOrNull())
            {
                entities = entities.Where(x => x.Categories.Any(c => query.CategoriesIds.Contains(c.CategoryId)));
            }

            if (!query.BrandsIds.IsEmptyOrNull())
            {
                entities = entities.Where(x => query.BrandsIds.Contains(x.BrandId));
            }

            if (!query.Colors.IsEmptyOrNull())
            {
                entities = entities.Where(x => x.Variants.Any(v => query.Colors.Contains(v.Color)));
            }

            if (!query.SizesIds.IsEmptyOrNull())
            {
                entities = entities.Where(x => x.Variants.Any(v => query.SizesIds.Contains(v.SizeId)));
            }

            entities = entities.Where(x => x.SalePrice >= query.MinPrice);
            entities = entities.Where(x => x.SalePrice <= query.MaxPrice);
            entities = entities.Where(x => x.Videos.Any());
            entities = entities.DistinctBy(x => x.Videos.First());

            switch (query.Filter)
            {
                case SearchForProductVideosFilters.Relevance:
                    break;
                case SearchForProductVideosFilters.Newest:
                    entities = entities.OrderByDescending(x => x.CreatedAt);
                    break;
                case SearchForProductVideosFilters.LowToHighPrice:
                    entities = entities.OrderBy(x => x.SalePrice);
                    break;
                case SearchForProductVideosFilters.HighToLowPrice:
                    entities = entities.OrderByDescending(x => x.SalePrice);
                    break;
            }

            var mapConfig = new TypeAdapterConfig();

            mapConfig.NewConfig<Product, SearchForProductVideosQueryDto>()
                .Map(dest => dest.VideoId, src => src.Videos.First().VideoId);

            var products = await entities
                .ProjectToType<SearchForProductVideosQueryDto>(mapConfig)
                .ToPaginatedListAsync(
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
                cancellationToken: cancellationToken);

            return products;
        }
    }
}
