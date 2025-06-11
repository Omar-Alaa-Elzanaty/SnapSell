using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Domain.Models.MongoDbEntities;

//public class ProductImage : BaseEntity
//{
//    public Guid ProductId { get; set; }
//    public virtual Product Product { get; set; }
//    public string ImageUrl { get; set; }
//    public bool IsMainImage { get; set; }
//}


public class ProductImage
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMainImage { get; set; }
}