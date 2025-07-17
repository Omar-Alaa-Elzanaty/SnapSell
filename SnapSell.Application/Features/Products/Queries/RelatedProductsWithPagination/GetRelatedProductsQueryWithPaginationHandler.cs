using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Products.Queries.RelatedProductsWithPagination
{
    internal class GetRelatedProductsQueryWithPaginationHandler 
        : IRequestHandler<GetRelatedProductsQueryWithPagination, PaginatedResult<GetRelatedProductsQueryWithPaginationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<GetRelatedProductsQueryWithPaginationHandler> _localizer;

        public GetRelatedProductsQueryWithPaginationHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<GetRelatedProductsQueryWithPaginationHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetRelatedProductsQueryWithPaginationDto>> Handle(
            GetRelatedProductsQueryWithPagination query,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductsRepo.Entities
                        .FirstOrDefaultAsync(x => x.Id == query.Id,cancellationToken);

            if(product == null)
            {
                return PaginatedResult<GetRelatedProductsQueryWithPaginationDto>
                    .Failure(_localizer["ProductNotFound"], HttpStatusCode.NotFound);
            }

            var relatedProducts = await _unitOfWork.ProductsRepo.Entities
                        .Where(x =>
                        (x.Categories.Any(c => product.Categories.Contains(c)) && x.BrandId == product.BrandId && x.StoreId == product.StoreId)
                        || (x.Categories.Any(c => product.Categories.Contains(c)) && x.StoreId == product.StoreId)
                        || (x.Categories.Any(c => product.Categories.Contains(c)) && x.BrandId == product.BrandId)
                        || x.Categories.Any(c => product.Categories.Contains(c)))
                        .ProjectToType<GetRelatedProductsQueryWithPaginationDto>()
                        .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return relatedProducts;
        }
    }
}
