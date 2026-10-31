namespace BuildingBlocks.IntegrationEvent
{
    public sealed record BookingPaymentSucceededIntegrationEvent(
    Guid BookingId,
    string PaymentReference,
    decimal Amount,
    string Currency);


    public sealed record BookingConfirmedIntegrationEvent(
    Guid BookingId,
    Guid TenantId,
    Guid ProfessionalId,
    string CustomerId,
    Guid ServiceId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc);
}
