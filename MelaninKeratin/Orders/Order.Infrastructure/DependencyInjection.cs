using Marten;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Application.Services;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here            

            var connectionString = configuration.GetConnectionString("orderDb")
            ?? throw new InvalidOperationException("Order database connection string was not configured.");


            services.AddDbContext<OrderDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString(connectionString)));

            services.AddScoped<IOrderRepository, OrderRepository>();          

            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
