using Mapster;
using Microsoft.EntityFrameworkCore;
using Order.Application.Dtos;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Repositories
{
    public sealed class OrderRepository(OrderDbContext context) : IOrderRepository
    {
        public async Task<OrderResponse?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await context.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            return order?.Adapt<OrderResponse>();
        }

        public async Task<IReadOnlyList<OrderResponse>> GetByCustomerIdAsync( string customerId, CancellationToken cancellationToken)
        {
            var orders = await context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync(cancellationToken);


            return orders.Adapt<IReadOnlyList<OrderResponse>>();
        }

        public async Task AddAsync(CreateOrderRequest order, CancellationToken cancellationToken)
        {
            var orderToAdd = order.Adapt<Ordering>();
            await context.Orders.AddAsync(orderToAdd, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
