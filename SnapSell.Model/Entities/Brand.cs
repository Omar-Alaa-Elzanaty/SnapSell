using MongoDB.Bson.Serialization.Attributes;
using SnapSell.Domain.Attributes;

namespace SnapSell.Domain.Entities
{
    [BsonCollection("brands")]
    public class Brand : Document
    {
        public string Name { get; set; }
        public string Slug { get; set; } // For URLs: "nike" -> /brands/nike
        public string LogoUrl { get; set; }

        [BsonElement("product_ids")]
        public List<string> ProductIds { get; set; } = new();

        [BsonIgnore]
        public List<Product> Products { get; set; }
    }
}
