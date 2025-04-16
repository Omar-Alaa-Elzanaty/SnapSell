namespace SnapSell.Domain.Entities
{
    public sealed class Review
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string ProductId { get; set; } //references MongoDB product
        public int Rating { get; set; } //1-5
        public required string Title { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // N.P
        public required User User { get; set; }
    }
}
