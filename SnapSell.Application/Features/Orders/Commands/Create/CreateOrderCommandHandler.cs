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

namespace SnapSell.Application.Features.Orders.Commands.Create
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
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UserManager<Account> userManager)
        {
            _paymobService = paymobService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result<string>.ValidationFailure(validationResult.Errors);
            }

            var clientId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (clientId is null)
            {
                return Result<string>.Failure("User not found");
            }
            
            var client = await _userManager.FindByIdAsync(clientId);

            var amount = (double)await _unitOfWork.VariantsRepo.Entities
                .Where(v => command.Varients.Select(x => x.VariantId).Contains(v.Id))
                .SumAsync(p => p.SalePrice, cancellationToken);

            var result = await _paymobService.CreatePayment(new PaymobIntenstionRequestDto()
            {
                Amount = amount,
                BillingData = new PaymobBillingDto()
                {
                    Country = client.Country,
                    Email = client.Email,
                    FirstName = client.FirstName.Split(' ')[0],
                    LastName = client.LastName.Split(' ').Skip(1).Take(1).FirstOrDefault() ?? "",
                    PhoneNumber = client.PhoneNumber!
                },
                Currency = command.Currency.ToString(),
                PaymentMethods = [_configuration[$"Paymob:PaymentMethods:{command.PaymentMethod.ToString()}"]!],
            });

            var order = command.Adapt<Order>();

            order.AccountId = clientId;
            order.Email = client.Email;
            order.OrderTotal = (decimal)amount;
            order.PaymobOrderId = result.PaymentKeys.First().OrderId;
            order.Items = command.Varients.Adapt<List<OrderItem>>();

            await _unitOfWork.OrdersRepo.AddAsync(order);
            await _unitOfWork.SaveAsync(cancellationToken);


            var payment = new Payment
            {
                OrderId = order.Id,
                IntegrationId = result.PaymentKeys.First().Integration,
                PaymentMethod = command.PaymentMethod
            };

            await _unitOfWork.PaymentsRepo.AddAsync(payment);
            await _unitOfWork.SaveAsync(cancellationToken);

            var redirectUrl = _configuration["Paymob:RedirectUrl"]
                + _configuration["Paymob:PublicKey"]
                + "&clientSecret=" + result.ClientSecret;

            return Result<string>.Success(data: redirectUrl);
        }
    }
}
