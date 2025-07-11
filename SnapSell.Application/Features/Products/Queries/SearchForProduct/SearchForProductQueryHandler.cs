using FluentValidation;
using Mapster;
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

            var entities = _unitOfWork.ProductsRepo.Entities;

            SortDefinition<Product> sort = null!;

            if (!command.CategoriesIds.IsEmptyOrNull())
            {
                entities = entities.Where(x => x.CategoryIds.Any(c => command.CategoriesIds.Contains(c)));
            }

            if (!command.BrandsIds.IsEmptyOrNull())
            {
                entities = entities.Where(x => command.BrandsIds.Contains(x.BrandId));
            }

            if (!command.Colors.IsEmptyOrNull())
            {
                entities= entities.Where(x => x.Variants.Any(v => command.Colors.Contains(v.Color)));
            }

            if (!command.SizesIds.IsEmptyOrNull())
            {
                entities= entities.Where(x => x.Variants.Any(v => command.SizesIds.Contains(v.SizeId)));
            }

            entities= entities.Where(x => x.SalePrice >= command.MinPrice);
            entities= entities.Where(x => x.SalePrice <= command.MaxPrice);
            switch (command.Filter)
            {
                case SearchForProductSorts.Relevance:
                    break;
                case SearchForProductSorts.Newest:
                    entities= entities.OrderByDescending(x => x.CreatedAt);
                    break;
                case SearchForProductSorts.LowToHighPrice:
                    entities= entities.OrderBy(x => x.SalePrice);
                    break;
                case SearchForProductSorts.HighToLowPrice:
                    entities= entities.OrderByDescending(x => x.SalePrice);
                    break;
            }

            var mapConfig = new TypeAdapterConfig();
            mapConfig.NewConfig<Product, SearchForProductQueryDto>()
                .Map(dest => dest.ImageUrl, src => src.Images.Where(i => i.IsMainImage).Select(i => i.ImageUrl).FirstOrDefault()!);

            var products = await entities
                .ProjectToType<SearchForProductQueryDto>(mapConfig)
                .ToPaginatedListAsync(
                    pageNumber: command.PageNumber,
                    pageSize: command.PageSize,
                    cancellationToken: cancellationToken);

            return products;
        }
    }
}
