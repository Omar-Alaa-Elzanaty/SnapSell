using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Extensions.Services;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Security.Claims;

namespace SnapSell.Application.Features.Payments.Queries.Checkout
{
    public class CheckoutQueryHandler : IRequestHandler<CheckoutQuery, Result<List<CheckoutQueryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IStringLocalizer<CheckoutQueryHandler> _localizer;

        public CheckoutQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IStringLocalizer<CheckoutQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _localizer = localizer;
        }

        public async Task<Result<List<CheckoutQueryDto>>> Handle(CheckoutQuery request, CancellationToken cancellationToken)
        {
            var clientId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value!;

            var addresses = await _unitOfWork.OrderAddressesRepo.Entities
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.IsDefault)
                .ProjectToType<CheckoutQueryDto>()
                .ToListAsync();

            if (addresses.IsEmptyOrNull())
            {
                return Result<List<CheckoutQueryDto>>.Failure(_localizer["ShouldEnterAddress"]);
            }

            return Result<List<CheckoutQueryDto>>.Success(addresses);
        }
    }
}
