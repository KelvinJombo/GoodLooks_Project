namespace Order.Domain.Enums
{
    public enum OrderStatus
    {
        PendingPayment,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        PaymentFailed
    }
}
