using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SnapSell.Domain.Entities
{
    [BsonCollection("products")]
    public sealed class Product : Document
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }

        // Category references
        public required string PrimaryCategoryId { get; set; }
        public List<string> CategoryIds { get; set; } = new();

        // Dynamic attributes
        [BsonElement("attributes")]
        public BsonDocument Attributes { get; set; } = new();

        // Variants
        public List<ProductVariant> Variants { get; set; } = new();

        // Media
        public List<ProductImage> Images { get; set; } = new();

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsFeatured { get; set; }
        
        [BsonElement("brand_ids")]
        public List<string> BrandIds { get; set; } = new(); // Can belong to multiple brands

        [BsonElement("primary_brand_id")]
        public required string PrimaryBrandId { get; set; } // Main brand for display

        [BsonIgnore]
        public List<Brand> Brands { get; set; } // Populated during queries
    }
}
