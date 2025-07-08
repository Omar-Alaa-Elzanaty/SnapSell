using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapSell.Application.Features.Products.Queries.GetProductById;
using SnapSell.Application.Features.Products.Queries.SearchForProduct;
using SnapSell.Application.Features.Products.Queries.SearchForProductVideos;
using SnapSell.Domain.Dtos.ResultDtos;

namespace SnapSell.Presentation.EndPoints
{
    public class PublicController(ISender sender) : ApiControllerBase
    {
        [HttpPost("Search")]
        public async Task<ActionResult<PaginatedResult<SearchForProductQueryDto>>> SearchForProducts([FromBody] SearchForProductQuery query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return await HandleMediatorResult(result);
        }

        [HttpPost("Search/videos")]
        public async Task<ActionResult<PaginatedResult<SearchForProductVideosQueryDto>>> SearchForProductVideos([FromBody] SearchForProductVideosQuery query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return await HandleMediatorResult(result);
        }

        [HttpGet("Products/{id}")]
        public async Task<ActionResult<Result<GetProductByIdQueryDto>>> GetProductById(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return await HandleMediatorResult(result);
        }
    }
}
