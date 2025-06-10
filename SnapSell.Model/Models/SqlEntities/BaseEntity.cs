using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnapSell.Domain.Models.SqlEntities;

public class BaseEntity : Auditable
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
}