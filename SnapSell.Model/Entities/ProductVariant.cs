namespace SnapSell.Domain.Entities
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public required string Color { get; set; }
        public required string Size { get; set; }
        public string? SKU { get; set; }
        //public int StockQuantity { get; set; }
        //public required string StockStatus { get; set; }
    }
}
