using Order.Application.Dtos;

namespace Order.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse?> GetOrderAsync( Guid orderId,  string customerId, CancellationToken cancellationToken);

        Task<IReadOnlyList<OrderResponse>> GetCustomerOrdersAsync( string customerId, CancellationToken cancellationToken);

        
    }
}
