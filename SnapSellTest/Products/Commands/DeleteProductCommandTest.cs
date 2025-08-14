using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Application.Features.Products.Commands.DeleteProduct;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;
using SnapSell.Test.FakeObjects;

namespace SnapSell.Test.Products.Commands;

public class DeleteProductCommandTest : TestBase
{
    private readonly Mock<IMediaService> _mediaServiceMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    [Fact]
    public async Task Handle_ShouldDeleteProduct_WhenProductExistsAndUserIsOwner()
    {
        // Arrange
        var unitOfWork = GetUnitOfWork();
        var userManager = _serviceProvider.GetRequiredService<UserManager<Account>>();

        //create user
        var user = await CreateTestUserAsync(userManager);
        SetupUserIdentityInHttpContext(user);

        //save related entities (store, brand, category)
        var store = CreateStore(user.Id);
        var brand = CreateBrand();
        var category = CreateCategory();
        await unitOfWork.StoresRepo.AddAsync(store);
        await unitOfWork.BrandsRepo.AddAsync(brand);
        await unitOfWork.CategoryRepo.AddAsync(category);
        await unitOfWork.SaveAsync();

        //create the product With store
        var product = CreateTestProduct(brand.Id, category.Id, store.Id);
        await unitOfWork.ProductsRepo.AddAsync(product);
        await unitOfWork.SaveAsync();

        var handler = new DeleteProductCommandHandler(
            unitOfWork,
            _httpContextAccessorMock.Object,
            _mediaServiceMock.Object);

        // Act
        var result = await handler.Handle(new DeleteProductCommand(product.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<Account> CreateTestUserAsync(UserManager<Account> userManager)
    {
        var user = new Account
        {
            FirstName = "Test",
            LastName = "User",
            Email = "testemail@gmail.com",
            UserName = "testemail@gmail.com"
        };

        var createUserResult = await userManager.CreateAsync(user, "123@Abc");
        createUserResult.Succeeded.Should().BeTrue();

        return user;
    }

    private void SetupUserIdentityInHttpContext(Account user)
    {
        var identity = new GenericIdentity(user.UserName!);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var httpContext = new DefaultHttpContext();
        httpContext.User.AddIdentity(identity);

        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);
    }

    private Store CreateStore(string userId)
    {
        return new Store
        {
            AccountId = userId,
            Name = "Test Store",
            Description = "Test Store Description"
        };
    }

    private Brand CreateBrand() => new Brand { Id = Guid.NewGuid(), Name = "Test Brand" };
    private Category CreateCategory() => new Category { Id = Guid.NewGuid(), Name = "Test Category" };
    private Product CreateTestProduct(Guid brandId, Guid categoryId, Guid storeId)
    {
        var product = new Product
        {
            BrandId = brandId,
            StoreId = storeId, 
            EnglishName = "Test Product",
            ArabicName = "تجربة المنتج",
            IsFeatured = false,
            IsHidden = false,
            ProductStatus = ProductStatus.Draft,
            HasVariants = false,
            ShippingType = ShippingType.Free,
            ProductType = ProductTypes.New,
            PaymentMethods = [1, 2, 3],
            Images =
            [
                new ProductImage
                {
                    ImageUrl = Constants.Base64Image,
                    IsMainImage = true
                }
            ],
            EnglishDescription = "English Description",
            ArabicDescription = "الوصف العربي",
            MinDeliveryDays = 1,
            MaxDeliveryDays = 3,
            Price = 100m,
            SalePrice = 80m,
            CostPrice = 50m,
            Quantity = 10,
            Sku = "SKU123",
            Categories = new List<ProductCategory>
            {
                new ProductCategory { CategoryId = categoryId }
            },
            Variants = null!
        };

        return product;
    }
}