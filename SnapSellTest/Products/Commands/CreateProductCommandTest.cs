using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnapSell.Application.Features.product.Commands.CreateProduct;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Test.Products.Commands
{
    public class CreateProductCommandTest : TestBase
    {
        public CreateProductCommandTest()
        {
        }

        [Fact]
        public async Task CreateProductHandler_WhenCreateProduct_ReturnSuccess()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = _serviceProvider.GetRequiredService<UserManager<Account>>();
            var httpContext = new DefaultHttpContext();

            var user = new Account()
            {
                FullName = "Test User",
                Email = "testemail@gmail.com"
            };

            await userManager.CreateAsync(user, "123@Abc");

            var identity = new GenericIdentity("User");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            httpContext.User.AddIdentity(identity);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext)
                               .Returns(httpContext);

            var brand = new Brand()
            {
                Name = "Brand"
            };

            await unitOfWork.BrandsRepo.AddAsync(brand);
            await unitOfWork.SaveAsync();

            //Act

            var validator = new CreatProductCommandValidator();

            var handler = new CreatProductCommandHandler(unitOfWork, httpContextAccessor.Object, validator);

            var result = await handler.Handle(new CreatProductCommand(
                brand.Id,
                "Test",
                "تجربة",
                false,
                false,
                ShippingType.Free,
                ProductStatus.New), default);

            //Assert

            result.IsSuccess.Should().BeTrue();
        }
    }
}
