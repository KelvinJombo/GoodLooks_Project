using Order.Application.Dtos;

namespace Order.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponse?> GetByIdAsync( Guid orderId, CancellationToken cancellationToken);

        Task<IReadOnlyList<OrderResponse>> GetByCustomerIdAsync( string customerId, CancellationToken cancellationToken);

        Task AddAsync(CreateOrderRequest order, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
