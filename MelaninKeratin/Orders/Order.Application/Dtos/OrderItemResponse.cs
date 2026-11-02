namespace Order.Application.Dtos
{
    public sealed record OrderItemResponse(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal SubTotal);
}
