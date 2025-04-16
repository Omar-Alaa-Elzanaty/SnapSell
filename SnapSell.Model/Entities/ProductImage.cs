namespace SnapSell.Domain.Entities
{
    public class ProductImage
    {
        public required string ImageId { get; set; }
        public required string Url { get; set; }
        public string? AltText { get; set; }
    }
}
