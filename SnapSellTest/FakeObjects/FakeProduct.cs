using Bogus;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Test.FakeObjects
{
    internal class FakeProduct
    {
        private static Faker<Product> CreateFakeObject()
        {
            var productImage = new Faker<ProductImage>()
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(x => x.IsMainImage, f => f.Random.Bool());

            return new Faker<Product>()
                .RuleFor(x => x.EnglishDescription, f => f.Lorem.Word())
                .RuleFor(x => x.ArabicDescription, f => f.Lorem.Word())
                .RuleFor(x => x.ArabicName, f => "ar" + f.Name.Suffix())
                .RuleFor(x => x.EnglishName, f => "en" + f.Name.Suffix())
                .RuleFor(x => x.CostPrice, f => f.PickRandom<decimal>())
                .RuleFor(x => x.Price, f => f.PickRandom<decimal>())
                .RuleFor(x => x.SalePrice, f => f.PickRandom<decimal>())
                .RuleFor(x => x.Images, productImage.Generate(new Random().Next(1, 5)));
        }

        public static List<Product> Create(int count = 1)
        {
            return CreateFakeObject().Generate(count);
        }

        public static Product Create()
        {
            return CreateFakeObject().Generate();
        }

        public static List<Product> Create(int min = 1, int max = 1)
        {
            return CreateFakeObject().GenerateBetween(min, max);
        }

    }
}
