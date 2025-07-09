using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;

namespace SnapSell.Application.Features.Products.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProductByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GetProductByIdQueryDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var lang = _httpContextAccessor.HttpContext!.Request.Headers.AcceptLanguage.ToString();

            var product = await _unitOfWork.ProductsRepo.Collection.Find(x => x.Id == query.Id)
                .Project(x => new GetProductByIdQueryDto()
                {
                    Id = x.Id,
                    Name = (lang == "ar" ? x.ArabicName : x.EnglishName) ?? "",
                    Price = x.Price,
                    SalePrice = x.SalePrice,
                    Description = (lang == "ar" ? x.ArabicDescription : x.EnglishDescription) ?? "",
                    ImagesUrl = x.Images.OrderByDescending(x => x.IsMainImage).Select(x => x.ImageUrl).ToList(),
                    MaxDeliveryDays = x.MaxDeliveryDays,
                    MinDeliveryDays = x.MinDeliveryDays,
                    BrandId = x.BrandId,
                    ProductType = x.ProductType,
                    ShippingType = x.ShippingType,
                    Variants = x.Variants.Select(v => new ProductVariantDto()
                    {
                        Color = v.Color,
                        Id = v.Id,
                        SizeId = v.SizeId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return Result<GetProductByIdQueryDto>.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            product.BrandName = await _unitOfWork.BrandsRepo.Entities
                .Where(x => x.Id == product.BrandId)
                .Select(x => x.Name)
                .FirstAsync(cancellationToken);

            var sizes = await _unitOfWork.SizesRepo.Entities
                        .Where(x => product.Variants.Select(x => x.Id).Contains(x.Id))
                        .Select(x => new { x.Id, x.Name })
                        .ToListAsync(cancellationToken);

            foreach (var variant in product.Variants)
            {
                variant.Size = sizes.First(x => x.Id == variant.SizeId).Name!;
            }

            return Result<GetProductByIdQueryDto>.Success(product);
        }
    }
}
