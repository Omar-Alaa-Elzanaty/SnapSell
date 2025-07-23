using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
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
        private readonly ILanguageHelper _langHelper;

        public GetRelatedProductsQueryWithPaginationHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<GetRelatedProductsQueryWithPaginationHandler> localizer,
            IHttpContextAccessor contextAccessor,
            ILanguageHelper langHelper)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _langHelper = langHelper;
        }

        public async Task<PaginatedResult<GetRelatedProductsQueryWithPaginationDto>> Handle(
            GetRelatedProductsQueryWithPagination query,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductsRepo.Entities
                        .FirstOrDefaultAsync(x => x.Id == query.Id,cancellationToken);

            var lang = _langHelper.GetLang();

            if(product == null)
            {
                return PaginatedResult<GetRelatedProductsQueryWithPaginationDto>
                    .Failure(_localizer["ProductNotFound"], HttpStatusCode.NotFound);
            }

            var config = new TypeAdapterConfig();
                config.NewConfig<Product, GetRelatedProductsQueryWithPaginationDto>()
                .Map(dest => dest.Name, src => lang == "ar" ? src.ArabicName : src.EnglishName)
                .Map(dest => dest.ImageUrl, src => src.Images.FirstOrDefault(x => x.IsMainImage));

            var relatedProducts = await _unitOfWork.ProductsRepo.Entities
                        .Where(x =>
                        (x.Categories.Any(c => product.Categories.Select(x => x.CategoryId).Contains(c.CategoryId)) && x.BrandId == product.BrandId && x.StoreId == product.StoreId)
                        || (x.Categories.Any(c => product.Categories.Select(x => x.CategoryId).Contains(c.CategoryId)) && x.StoreId == product.StoreId)
                        || (x.Categories.Any(c => product.Categories.Select(x => x.CategoryId).Contains(c.CategoryId)) && x.BrandId == product.BrandId)
                        || x.Categories.Any(c => product.Categories.Select(x => x.CategoryId).Contains(c.CategoryId)))
                        .ProjectToType<GetRelatedProductsQueryWithPaginationDto>(config)
                        .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return relatedProducts;
        }
    }
}
