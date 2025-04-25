using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Models
{
    public class Variant
    {
        public int Id { get; set; }
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public int colorId { get; set; }
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int RegularPrice { get; set; }
        public int? SalePrice { get; set; }
    }
}
