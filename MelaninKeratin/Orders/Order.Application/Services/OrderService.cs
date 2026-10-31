using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Application.Services
{
    public sealed class OrderService(
    IOrderRepository orderRepository) : IOrderService
    {
        public async Task<Ordering?> GetOrderAsync(
            Guid orderId,
            string customerId,
            CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(
                orderId, cancellationToken);

            if (order is null || order.CustomerId != customerId)
                return null;

            return order;
        }

        public Task<IReadOnlyList<Ordering>> GetCustomerOrdersAsync(
            string customerId,
            CancellationToken cancellationToken)
        {
            return orderRepository.GetByCustomerIdAsync(
                customerId, cancellationToken);
        }

        public async Task CancelOrderAsync(
            Guid orderId,
            string customerId,
            CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(
                orderId, cancellationToken);

            if (order is null || order.CustomerId != customerId)
                throw new KeyNotFoundException("Order was not found.");

            order.Cancel();

            await orderRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
