using Order.Domain.Entities;

namespace Order.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Ordering?> GetOrderAsync(
            Guid orderId,
            string customerId,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<Ordering>> GetCustomerOrdersAsync(
            string customerId,
            CancellationToken cancellationToken);

        Task CancelOrderAsync(
            Guid orderId,
            string customerId,
            CancellationToken cancellationToken);
    }
}
