namespace BuildingBlocks.IntegrationEvent
{
    public sealed record OrderPaidIntegrationEvent(Guid OrderId, string CustomerId, IReadOnlyCollection<SoldProduct> Items);

    public sealed record SoldProduct( Guid ProductId, int Quantity);
}
