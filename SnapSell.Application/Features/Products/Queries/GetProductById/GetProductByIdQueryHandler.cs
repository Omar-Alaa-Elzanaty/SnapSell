using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
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

            var mapConfig = new TypeAdapterConfig();
            mapConfig.NewConfig<Product, GetProductByIdQueryDto>()
                .Map(dest => dest.Name, src => lang == "ar" ? src.ArabicName : src.EnglishName)
                .Map(dest => dest.Description, src => lang == "ar" ? src.ArabicDescription : src.EnglishDescription)
                .Map(dest => dest.ImagesUrl, src => src.Images.OrderByDescending(x => x.IsMainImage).Select(x => x.ImageUrl))
                .Map(dest => dest.BrandName, src => src.Brand.Name);

            var product = await _unitOfWork.ProductsRepo.Entities
                .Where(x => x.Id == query.Id)
                .ProjectToType<GetProductByIdQueryDto>(mapConfig)
                .FirstOrDefaultAsync(cancellationToken); 

            if (product == null)
            {
                return Result<GetProductByIdQueryDto>.Failure("Product not found.", HttpStatusCode.NotFound);
            }

            return Result<GetProductByIdQueryDto>.Success(product);
        }
    }
}
