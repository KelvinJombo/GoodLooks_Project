namespace BuildingBlocks.IntegrationEvent
{
    public sealed record PaymentSucceededIntegrationEvent(
    Guid OrderId,
    string PaymentReference,
    decimal Amount,
    string Currency);
}
