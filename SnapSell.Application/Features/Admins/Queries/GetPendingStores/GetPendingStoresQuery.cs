using MediatR;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Dtos.ResultDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Admins.Queries.GetPendingStores
{
    public sealed record GetPendingStoresQuery:PaginatedRequest,IRequest<PaginatedResult<GetPendingStoresQueryDto>>;

    public class GetPendingStoresQueryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
    }
}
