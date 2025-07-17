using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Products.Queries.RelatedProductsWithPagination
{
    public record GetRelatedProductsQueryWithPagination:PaginatedRequest,IRequest<PaginatedResult<GetRelatedProductsQueryWithPaginationDto>>
    {
        public int Id { get; set; }
    }

    public class GetRelatedProductsQueryWithPaginationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
