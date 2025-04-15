using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SnapSell.Domain.Interfaces;

namespace SnapSell.Domain.Entities
{
    public abstract class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
