using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos.PaymobDtos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using System.Security.Claims;

namespace SnapSell.Application.Features.Orders.Commands
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<string>>
    {
        private readonly IPaymobService _paymobService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CreateOrderCommandHandler(
            IPaymobService paymobService,
            IUnitOfWork unitOfWork,
            IValidator<CreateOrderCommand> validator,
            UserManager<Account> userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _paymobService = paymobService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<Result<string>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if(!validationResult.IsValid)
            {
                return Result<string>.ValidationFailure(validationResult.Errors);
            }

            var clientId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var client = await _userManager.FindByIdAsync(clientId);

            var amount = (double)await _unitOfWork.VariantsRepo.Entities
                .Where(v => command.Varients.Select(x=>x.VariantId).Contains(v.Id))
                .SumAsync(p => p.SalePrice, cancellationToken);

            var result = await _paymobService.CreatePayment(new PaymobIntenstionRequestDto()
            {
                Amount = amount,
                BillingData = new PaymobBillingDto()
                {
                    Country = client.Country,
                    Email = client.Email,
                    FirstName = client.FullName.Split(' ')[0],
                    LastName = client.FullName.Split(' ').Skip(1).Take(1).FirstOrDefault() ?? "",
                    PhoneNumber = client.PhoneNumber!
                },
                Currency = command.Currency.ToString(),
                PaymentMethods = [_configuration[$"Paymob:PaymentMethods:{command.PaymentMethod.ToString()}"]!],
            });

            var order = command.Adapt<Order>();

            order.ClientId = clientId;
            order.Email = client.Email;
            order.OrderTotal = (decimal)amount;
            order.Items = command.Varients.Adapt<List<OrderItem>>();


            await _unitOfWork.OrdersRepo.AddAsync(order);
            await _unitOfWork.SaveAsync(cancellationToken);

            var redirectUrl = _configuration["Paymob:RedirectUrl"]!.Replace("ClientSecretValue", result.ClientSecret);

            return Result<string>.Success(data: redirectUrl);
        }
    }
}
