using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SnapSell.Domain.Attributes;

namespace SnapSell.Domain.Entities
{
    [BsonCollection("products")]
    public sealed class Product : Document
    {
        public required string EnglishName { get; set; }
        public required string ArabicName { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }

        // Category
        public required string PrimaryCategoryId { get; set; }
        public List<string> CategoryIds { get; set; } = new();

        [BsonElement("attributes")]
        public BsonDocument Attributes { get; set; } = new();
        public List<ProductVariant> Variants { get; set; } = new();
        public List<ProductImage> Images { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsFeatured { get; set; }
      
        [BsonElement("brand_id")]
        public required string BrandId { get; set; }

        [BsonIgnore]
        public List<Brand> Brands { get; set; }
    }
}