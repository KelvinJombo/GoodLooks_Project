namespace Payment.Api.Services;

public interface IOrderPaymentLookup
{
    Task<OrderPaymentDetails?> GetPayableOrderAsync(string orderId, string customerId, CancellationToken cancellationToken);
}


public sealed record OrderPaymentDetails(string OrderId, string CustomerId, string CustomerEmail, decimal Amount, string Currency, bool IsPayable);
