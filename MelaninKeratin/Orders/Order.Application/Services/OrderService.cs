using Order.Application.Dtos;
using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Application.Services
{
    public sealed class OrderService( IOrderRepository orderRepository) : IOrderService
    {
        public async Task<OrderResponse?> GetOrderAsync(Guid orderId, string customerId, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order is null || order.CustomerId != customerId)
                return null;

            return order;
        }

        public Task<IReadOnlyList<OrderResponse>> GetCustomerOrdersAsync(string customerId, CancellationToken cancellationToken)
        {
            return orderRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        }

        
    }
}
