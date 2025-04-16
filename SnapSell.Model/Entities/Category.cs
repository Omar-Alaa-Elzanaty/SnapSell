using SnapSell.Domain.Attributes;

namespace SnapSell.Domain.Entities
{
    [BsonCollection("categories")]
    public class Category : Document
    {
        public string Name { get; set; }
        public required string Description { get; set; }
        public string? ParentCategoryId { get; set; }
        public List<string> AncestorIds { get; set; } = new();
        public string? Path { get; set; } // e.g., "Electronics/Computers/Laptops"
    }
}