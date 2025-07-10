using FluentValidation;
using MediatR;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Application.Extensions.Services;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Application.Features.Products.Queries.SearchForProduct
{
    internal class SearchForProductQueryHandler : IRequestHandler<SearchForProductQuery, PaginatedResult<SearchForProductQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SearchForProductQuery> _validator;

        public SearchForProductQueryHandler(
            IUnitOfWork unitOfWork,
            IValidator<SearchForProductQuery> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<PaginatedResult<SearchForProductQueryDto>> Handle(SearchForProductQuery command, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
            {
                return PaginatedResult<SearchForProductQueryDto>.ValidationFailure(validation.Errors);
            }

            var filterBuilder = Builders<Product>.Filter;

            var filters = FilterDefinition<Product>.Empty;

            SortDefinition<Product> sort = null!;

            if (!command.CategoriesIds.IsEmptyOrNull())
            {
                filters = Builders<Product>.Filter.AnyIn(x => x.CategoryIds, command.CategoriesIds);
            }

            if (!command.BrandsIds.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.In(x => x.BrandId, command.BrandsIds);
            }

            if (!command.Colors.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.ElemMatch(x => x.Variants, Builders<Variant>.Filter.In(v => v.Color, command.Colors));
            }

            if (!command.SizesIds.IsEmptyOrNull())
            {
                filters &= Builders<Product>.Filter.ElemMatch(x => x.Variants, Builders<Variant>.Filter.In(v => v.SizeId, command.SizesIds));
            }

            filters &= Builders<Product>.Filter.Gte(x => x.SalePrice, command.MinPrice);
            filters &= Builders<Product>.Filter.Lte(x => x.SalePrice, command.MaxPrice);

            var sortBuilder = Builders<Product>.Sort;
            switch (command.Filter)
            {
                case SearchForProductSorts.Relevance:
                    break;
                case SearchForProductSorts.Newest:
                    sort = sortBuilder.Descending(x => x.CreatedAt);
                    break;
                case SearchForProductSorts.LowToHighPrice:
                    sort = sortBuilder.Ascending(x => x.SalePrice);
                    break;
                case SearchForProductSorts.HighToLowPrice:
                    sort = sortBuilder.Ascending(x => x.SalePrice);
                    break;
            }

            var projection = Builders<Product>.Projection.Expression(p =>
            new SearchForProductQueryDto
            {
                Id = p.Id,
                Price = p.Price,
                SalePrice = p.SalePrice,
                ImageUrl = p.Images.Where(i => i.IsMainImage).Select(i => i.ImageUrl).FirstOrDefault()!
            });


            var products = await _unitOfWork.ProductsRepo.Collection
                .ToPaginatedListAsync(
                    filter: filters,
                    sort: sort,
                    projection: projection,
                    pageNumber: command.PageNumber,
                    pageSize: command.PageSize,
                    cancellationToken: cancellationToken);

            return products;
        }
    }
}
