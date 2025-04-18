namespace SnapSell.Domain.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public required string ProductId { get; set; } // References MongoDB product
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}
