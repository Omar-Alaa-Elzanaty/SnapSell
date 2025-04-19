namespace SnapSell.Domain.Entities
{
    public class SellerInventory
    {
        public Guid Id { get; set; }
        public required string SellerId { get; set; } // may remove 
        public Product Product { get; set; }
        public required Guid ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }
        public ProductVariant Variant { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public string StockStatus => AvailableQuantity > 0 ? "In Stock" : "Out of Stock";
    }
}