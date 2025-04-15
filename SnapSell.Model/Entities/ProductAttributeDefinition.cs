namespace SnapSell.Domain.Entities
{ 
    [BsonCollection("product_attributes")]
    public class ProductAttributeDefinition : Document
    {
        public string Name { get; set; }
        public string DataType { get; set; } // "text", "number", "boolean", "options"
        public List<string> Options { get; set; } = new(); // For dropdowns
        public bool IsRequired { get; set; }
        public List<string> ApplicableCategoryIds { get; set; } = new();
    }
}
