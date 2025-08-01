namespace SnapSell.Domain.Enums
{
    public enum PaymentStatus : byte
    {
        Pending = 0,
        Success = 1,
        Failed = 2,
        Refunded = 3,
        Cancelled = 4
    }
}
