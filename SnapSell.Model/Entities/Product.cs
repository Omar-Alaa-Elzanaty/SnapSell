namespace SnapSell.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int DeliveryDays { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
    }
}
