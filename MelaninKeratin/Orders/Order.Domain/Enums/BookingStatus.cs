namespace Order.Domain.Enums
{
    public enum BookingStatus
    {
        PendingPayment,
        Confirmed,
        Completed,
        Cancelled,
        PaymentFailed
    }
}
