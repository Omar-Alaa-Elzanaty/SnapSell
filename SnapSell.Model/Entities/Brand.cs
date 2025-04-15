using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Entities
{
    [BsonCollection("brands")]
    public class Brand : Document
    {
        public string Name { get; set; }
        public string Slug { get; set; } // For URLs: "nike" -> /brands/nike
        public string LogoUrl { get; set; }

        [BsonElement("product_ids")]
        public List<string> ProductIds { get; set; } = new(); // References to products

        [BsonIgnore]
        public List<Product> Products { get; set; } // Populated during queries
    }
}
