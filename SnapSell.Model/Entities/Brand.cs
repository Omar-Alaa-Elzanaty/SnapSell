namespace SnapSell.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Identifier { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public int AverageDeliveryDays { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
