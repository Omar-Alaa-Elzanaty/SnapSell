
﻿namespace SnapSell.Domain.Models
{
     public class Product : Auditable
    {
         [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }

        public List<Category> Categories { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
        public List<Variant> Variants { get; set; }
        public List<Review> Reviews { get; set; }

        public int Shipping { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsHidden { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int MinDeleveryDays { get; set; }
        public int MaxDeleveryDays { get; set; }
// =======
// ﻿using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;

// namespace SnapSell.Domain.Models
// {
//     public class Product : Auditable
//     {
        
//         public string Title { get; set; }
//         public List<ProductModel> Models { get; set; }
//     }

//     public class ProductModel
//     {
//         [BsonIgnoreIfDefault]
//         [BsonIgnoreIfNull]
//         public string Color { get; set; }
//         [BsonIgnoreIfDefault]
//         [BsonIgnoreIfNull]
//         public string Size { get; set; }
//         public int Quantity { get; set; }
// >>>>>>> main
    }
}
