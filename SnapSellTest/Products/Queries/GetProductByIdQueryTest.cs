using FluentAssertions;
using SnapSell.Application.Features.Products.Queries.GetProductById;
using SnapSell.Test.FakeObjects;

namespace SnapSell.Test.Products.Queries
{
    public class GetProductByIdQueryTest : TestBase
    {
        [Fact]
        public async Task Handler_WhenGetProductById_ReturnSuccess()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var pro = FakeProduct.Create();
            pro.HasVariants = true;
            await unitOfWork.ProductsRepo.InsertOneAsync(pro);

            //Act
            var result = await new GetProductByIdQueryHandler(unitOfWork, null).Handle(new GetProductByIdQuery(pro.Id), default);
            //Assert

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

    }
}
