using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SnapSell.Domain.Attributes;
using SnapSell.Domain.Enums;
using SnapSell.Domain.Models.SqlEntities;

namespace SnapSell.Domain.Models.MongoDbEntities;

[CollectionName("Products")]
public class Product:Auditable
{
    [BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }

    [BsonElement("englishName")]
    public string EnglishName { get; set; }
        
    [BsonElement("arabicName")]
    public string ArabicName { get; set; }
    
    [BsonElement("storeId")]
    public Guid StoreId { get; set; }

    [BsonElement("englishDescription")]
    public string? EnglishDescription { get; set; }

    [BsonElement("arabicDescription")]
    public string? ArabicDescription { get; set; }

    [BsonElement("categoryIds")]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> CategoryIds { get; set; } = new();

    [BsonElement("variants")]
    public List<Variant> Variants { get; set; } = new();

    [BsonElement("paymentMethodIds")]
    [BsonRepresentation(BsonType.Int32)]
    public List<int> PaymentMethods { get; set; } = new();

    [BsonElement("isFeatured")]
    public bool IsFeatured { get; set; }

    [BsonElement("isHidden")]
    public bool IsHidden { get; set; }

    [BsonElement("hasVariants")]
    public bool HasVariants { get; set; }

    [BsonElement("brandId")]
    public Guid BrandId { get; set; }

    [BsonElement("productStatus")]
    [BsonRepresentation(BsonType.Int32)]
    public ProductStatus ProductStatus { get; set; }

    [BsonElement("productType")]
    [BsonRepresentation(BsonType.Int32)]
    public ProductTypes ProductType { get; set; }

    [BsonElement("minDeliveryDays")]
    public int MinDeliveryDays { get; set; }

    [BsonElement("maxDeliveryDays")]
    public int MaxDeliveryDays { get; set; }

    [BsonElement("images")]
    public List<ProductImage> Images { get; set; } = new();

    [BsonElement("mainVideoUrl")]
    public string? MainVideoUrl { get; set; }

    [BsonElement("shippingType")]
    [BsonRepresentation(BsonType.Int32)]
    public ShippingType ShippingType { get; set; }

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
    [BsonElement("isDraft")]
    public bool IsDraft { get; set; }
}