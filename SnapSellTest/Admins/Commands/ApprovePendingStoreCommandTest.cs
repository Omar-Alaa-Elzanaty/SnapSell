using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using SnapSell.Application.Features.Admins.Commands.ApprovePendingStore;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;
using SnapSell.Domain.Models.SqlEntities.Identitiy;

namespace SnapSell.Test.Admins.Commands
{
    public class ApprovePendingStoreCommandTest:TestBase
    {
        public ApprovePendingStoreCommandTest()
        {
            
        }

        [Fact]
        public async Task Handler_WhenApprovePendingStore_ReturnSuccess()
        {
            // Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = _serviceProvider.GetRequiredService<UserManager<Account>>();

            var seller = new Seller() { FullName = "Test Seller", Email = "Test@gmail.com", UserName = "TestSeller" };
            await userManager.CreateAsync(seller, "123@Abc");

            var store = new Store
            {
                Name = "Test Store",
                Status = StoreStatusTypes.Pending,
                SellerId = seller.Id,
                DeliverPeriodTypes= DeliverPeriodTypes.Days,
                Description = "Test Store Description",
                LogoUrl="https://example.com/logo.png",
                MaximumDeliverPeriod= 3,
                MinimumDeliverPeriod = 1
            };

            await unitOfWork.StoresRepo.AddAsync(store);
            await unitOfWork.SaveAsync();

            var command = new ApprovePendingStoreCommand(store.Id);

            var localizer = _serviceProvider.GetRequiredService<IStringLocalizer<ApprovePendingStoreCommandHandler>>();

            var handler = new ApprovePendingStoreCommandHandler(unitOfWork, localizer);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            store.Status.Should().Be(StoreStatusTypes.Verified);

        }


        [Fact]
        public async Task Handler_WhenStoreNotFound_ReturnFail()
        {
            // Arrange
            var unitOfWork = GetUnitOfWork();

            var command = new ApprovePendingStoreCommand(Guid.NewGuid());

            var localizer = _serviceProvider.GetRequiredService<IStringLocalizer<ApprovePendingStoreCommandHandler>>();

            var handler = new ApprovePendingStoreCommandHandler(unitOfWork, localizer);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
