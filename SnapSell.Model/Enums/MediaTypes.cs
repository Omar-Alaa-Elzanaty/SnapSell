namespace SnapSell.Domain.Enums;

public static class ShippingConstants
{
    public const double BaseShippingCost = 50;
}

public enum Currency
{
    EgyptianPound = 1, // EGP
    SaudiRiyal = 2, // SAR
    KuwaitiDinar = 3, // KWD
    BahrainiDinar = 4, // BHD
    QatariRiyal = 5, // QAR
    OmaniRial = 6, // OMR
}

public enum MediaTypes : byte
{
    Image = 1,
    Video = 2
}

public enum ShippingType
{
    Paid = 1,
    Free = 2
}

public enum ProductStatus
{
    Used = 1,
    New = 2
}