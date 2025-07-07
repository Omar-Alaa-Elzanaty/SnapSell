using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Application.Features.Products.Queries.SearchForProductVideos
{
    public record SearchForProductVideosQuery : PaginatedRequest, IRequest<PaginatedResult<SearchForProductVideosQueryDto>>
    {
        public IEnumerable<Guid>? CategoriesIds { get; set; }
        public IEnumerable<Guid>? BrandsIds { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public IEnumerable<string>? Colors { get; set; }
        public IEnumerable<Guid>? SizesIds { get; set; }
        public SearchForProductVideosFilters Filter { get; set; }
    }

    public enum SearchForProductVideosFilters
    {
        Relevance = 1,
        Newest = 2,
        LowToHighPrice = 3,
        HighToLowPrice = 4
    }

    public class SearchForProductVideosQueryDto
    {
        public string VideoUrl { get; set; }
        public int ProductId { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Price { get; set; }
    }
}
