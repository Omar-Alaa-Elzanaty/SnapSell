using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Models.MongoDbEntities;

public class Variant
{
    [BsonElement("id")]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("productId")]
    [BsonRepresentation(BsonType.String)]
    public Guid ProductId { get; set; }

    [BsonElement("sizeId")]
    [BsonRepresentation(BsonType.String)]
    public Guid SizeId { get; set; }

    [BsonElement("color")]
    public string Color { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int? Quantity { get; set; }

    [BsonElement("price")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? Price { get; set; }

    [BsonElement("salePrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? SalePrice { get; set; }

    [BsonElement("costPrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? CostPrice { get; set; }

    [BsonElement("sku")]
    public string? Sku { get; set; }

    [BsonElement("isDefault")]
    public bool IsDefault { get; set; }
}