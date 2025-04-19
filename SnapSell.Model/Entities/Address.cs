namespace SnapSell.Domain.Entities
{
    public sealed class Address
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public required string StreetName { get; set; }
        public required string BuildingDetails { get; set; }
        public required string Country { get; set; }
        public required string Governorate { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public string? Landmark { get; set; }
        public bool IsDefault { get; set; }
    }
}
