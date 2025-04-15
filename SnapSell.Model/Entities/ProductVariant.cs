using MongoDB.Bson;

namespace SnapSell.Domain.Entities
{
    public class ProductVariant
    {
        public string VariantId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal PriceAdjustment { get; set; }
        public BsonDocument Attributes { get; set; } = new();
    }
}
