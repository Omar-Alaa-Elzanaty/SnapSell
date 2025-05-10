namespace SnapSell.Application.DTOs.payment;

public sealed record GetAllPaymentMethodsResponse(Guid PaymentMethodId, string Name);