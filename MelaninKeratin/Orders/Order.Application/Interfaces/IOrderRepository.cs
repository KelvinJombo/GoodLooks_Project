using Order.Domain.Entities;

namespace Order.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Ordering?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<Ordering>> GetByCustomerIdAsync(
            string customerId,
            CancellationToken cancellationToken);

        Task AddAsync(
            Ordering order,
            CancellationToken cancellationToken);

        Task SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
