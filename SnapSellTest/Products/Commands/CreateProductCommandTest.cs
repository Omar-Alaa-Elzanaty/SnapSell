using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Features.products.Commands.CreateProduct;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Test.FakeObjects;

namespace SnapSell.Test.Products.Commands;

public class CreateProductCommandTest : TestBase
{
    [Fact]
    public async Task CreateProductHandler_WhenValidData_ShouldReturnSuccess()
    {
        // Arrange
        var unitOfWork = GetUnitOfWork();
        var userManager = _serviceProvider.GetRequiredService<UserManager<Account>>();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();

        var mediaService = new Mock<IMediaService>();
        var user = new Account
        {
            FirstName = "Test",
            LastName = "User",
            Email = "testemail@gmail.com",
            UserName = "testemail@gmail.com"
        };

        var createUserResult = await userManager.CreateAsync(user, "123@Abc");
        createUserResult.Succeeded.Should().BeTrue();
        
        var identity = new GenericIdentity(user.UserName);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var httpContext = new DefaultHttpContext();
        httpContext.User.AddIdentity(identity);

        var store = new Store
        {
            AccountId = user.Id,
            Name = "Test Store",
            Description = "Test Store Description"
        };

        await unitOfWork.StoresRepo.AddAsync(store);

        var brand = new Brand
        {
            Id = Guid.NewGuid(),
            Name = "Brand"
        };
        await unitOfWork.BrandsRepo.AddAsync(brand);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Category"
        };
        await unitOfWork.CategoryRepo.AddAsync(category);
        
        await unitOfWork.SaveAsync();
        
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        mediaService
            .Setup(m => m.SaveAsync(It.IsAny<ProductImageDto>(), MediaTypes.Image))
            .ReturnsAsync("saved-image.jpg");

        var command = new CreateProductCommand(
            brand.Id,
            [category.Id],
            "Test Product",
            "تجربة المنتج",
            false,
            false,
            ProductStatus.Draft,
            false,
            (int)ShippingType.Free,
            ProductTypes.New,
            [(int)PaymentMethods.Cash],
            [
                new ProductImageDto
                {
                    FileName = "image.jpg",
                    Base64 = Constants.Base64Image,
                    IsMain = true
                }
            ],
            "English Description",
            "الوصف العربي",
            1,
            3,
            100m,
            80m,
            50m,
            10,
            "SKU123",
            null);

        var handler = new CreateProductCommandHandler(
            unitOfWork,
            mediaService.Object,
            httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}