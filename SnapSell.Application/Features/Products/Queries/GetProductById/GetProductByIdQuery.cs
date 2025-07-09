using MediatR;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Enums;
using System.Text.Json.Serialization;

namespace SnapSell.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery : IRequest<Result<GetProductByIdQueryDto>>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetProductByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Price { get; set; }
        public int MinDeliveryDays { get; set; }
        public int MaxDeliveryDays { get; set; }
        public List<string> ImagesUrl { get; set; }
        [JsonIgnore]
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public ProductTypes ProductType { get; set; }
        public ShippingType ShippingType { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
    }

    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid SizeId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }

    }
}
