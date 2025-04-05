using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Models
{
    public class Product : Auditable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public List<ProductModel> Models { get; set; }
    }

    public class ProductModel
    {
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Color { get; set; }
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public string Size { get; set; }
        public int Quantity { get; set; }
    }
}
