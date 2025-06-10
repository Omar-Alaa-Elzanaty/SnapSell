using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Models.SqlEntities;

public class Size:BaseEntity
{
    [BsonElement("name")]
    public string? Name { get; set; }
    [BsonElement("parentSizeId")]
    public Guid ParentSizeId { get; set; }
}