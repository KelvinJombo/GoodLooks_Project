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
            services.AddMarten(opts =>
            {
                opts.Connection(configuration.GetConnectionString("orderdb")!);
                //opts.Schema.For<Order>().Identity(x => x.UserName);
            }).UseLightweightSessions();

            services.AddDbContext<OrderDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("orderdb")));

            services.AddScoped<IOrderRepository, OrderRepository>();            
          

            //services.AddScoped<IOrderDbContext, OrderDbContext>();
            return services;
        }
    }
}
