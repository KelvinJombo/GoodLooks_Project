using Order.Domain.Enums;

namespace Order.Application.Dtos
{
    public sealed record OrderResponse
(
    Guid Id,
    string CustomerId,
    OrderStatus Status,
    string? PaymentReference,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAtUtc,
    DateTime? ConfirmedAtUtc,
    IReadOnlyCollection<OrderItemResponse> Items
);
}
