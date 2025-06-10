using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SnapSell.Domain.Attributes;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Domain.Models.MongoDbEntities;

//public class Product : BaseEntity
//{
//    public required string EnglishName { get; set; }
//    public required string ArabicName { get; set; }
//    public string? EnglishDescription { get; set; }
//    public string? ArabicDescription { get; set; }
//    public virtual List<Category> Categories { get; set; }
//    public virtual ICollection<Variant> Variants { get; set; } = new HashSet<Variant>();
//    public virtual ICollection<ProductPaymentMethod> ProductPaymentMethods { get; set; } = new HashSet<ProductPaymentMethod>();
//    public virtual List<Review> Reviews { get; set; }
//    public bool IsFeatured { get; set; }
//    public bool IsHidden { get; set; }
//    public bool HasVariants { get; set; }
//    public Guid BrandId { get; set; }
//    public virtual Brand Brand { get; set; }
//    public ProductStatus? ProductStatus { get; set; }
//    public int MinDeleveryDays { get; set; }
//    public int MaxDeleveryDays { get; set; }
//    public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();
//    public string? MainVideoUrl { get; set; }
//    public ShippingType? ShippingType { get; set; }

//    //for products without variants
//    public decimal? Price { get; set; }
//    public decimal? SalePrice { get; set; }
//    public decimal? CostPrice { get; set; }
//    public int? Quantity { get; set; }
//    public string? Sku { get; set; }
//}


[CollectionName("Products")]
public class Product : BaseEntity
{
    [BsonElement("englishName")]
    public required string EnglishName { get; set; }

    [BsonElement("arabicName")]
    public required string ArabicName { get; set; }

    [BsonElement("englishDescription")]
    public string? EnglishDescription { get; set; }

    [BsonElement("arabicDescription")]
    public string? ArabicDescription { get; set; }

    [BsonElement("categoryIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> CategoryIds { get; set; } = new();

    [BsonElement("variants")]
    public virtual List<Variant> Variants { get; set; } = new();

    [BsonElement("paymentMethodIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> PaymentMethodIds { get; set; } = new();

    [BsonElement("isFeatured")]
    public bool IsFeatured { get; set; }

    [BsonElement("isHidden")]
    public bool IsHidden { get; set; }

    [BsonElement("hasVariants")]
    public bool HasVariants { get; set; }

    [BsonElement("brandId")]
    public Guid BrandId { get; set; }

    [BsonElement("productStatus")]
    [BsonRepresentation(BsonType.String)]
    public ProductStatus? ProductStatus { get; set; }

    [BsonElement("minDeliveryDays")]
    public int MinDeliveryDays { get; set; }

    [BsonElement("maxDeliveryDays")]
    public int MaxDeliveryDays { get; set; }

    [BsonElement("images")]
    public virtual List<ProductImage> Images { get; set; } = new();

    [BsonElement("mainVideoUrl")]
    public string? MainVideoUrl { get; set; }

    [BsonElement("shippingType")]
    [BsonRepresentation(BsonType.String)]
    public ShippingType? ShippingType { get; set; }

    [BsonElement("price")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? Price { get; set; }

    [BsonElement("salePrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? SalePrice { get; set; }

    [BsonElement("costPrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? CostPrice { get; set; }

    [BsonElement("quantity")]
    public int? Quantity { get; set; }

    [BsonElement("sku")]
    public string? Sku { get; set; }
}