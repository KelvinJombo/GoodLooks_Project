using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Repositories
{
    public sealed class OrderRepository(
    OrderDbContext context) : IOrderRepository
    {
        public async Task<Ordering?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            return await context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<Ordering>> GetByCustomerIdAsync(
            string customerId,
            CancellationToken cancellationToken)
        {
            return await context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(
            Ordering order,
            CancellationToken cancellationToken)
        {
            await context.Orders.AddAsync(order, cancellationToken);
        }

        public Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
