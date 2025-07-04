using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Queries.SearchForProduct
{
    public record SearchForProductQuery : PaginatedRequest, IRequest<PaginatedResult<SearchForProductQueryDto>>
    {
        public IEnumerable<Guid>? CategoriesIds { get; set; }
        public IEnumerable<Guid>? BrandsIds { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IEnumerable<string>? Colors { get; set; }
        public IEnumerable<Guid>? SizesIds { get; set; }
        public SearchForProductSorts Filter { get; set; }
    }

    public enum SearchForProductSorts
    {
        Relevance = 1,
        Newest = 2,
        LowToHighPrice = 3,
        HighToLowPrice = 4
    }
    public class SearchForProductQueryDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
    }
}
