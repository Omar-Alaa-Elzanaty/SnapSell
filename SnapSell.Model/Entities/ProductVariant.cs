using MongoDB.Bson;

namespace SnapSell.Domain.Entities
{
    public class ProductVariant
    {
        public required string VariantId { get; set; }
        public required string Name { get; set; }
        public required string SKU { get; set; }
        public decimal PriceAdjustment { get; set; }
        public BsonDocument Attributes { get; set; } = new();
    }
}
