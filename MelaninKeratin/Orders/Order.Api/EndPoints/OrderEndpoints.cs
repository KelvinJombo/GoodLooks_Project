using Carter;
using Order.Application.Interfaces;
using System.Security.Claims;

namespace Order.Api.EndPoints
{
    public sealed class OrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/orders").WithTags("Orders").RequireAuthorization();

            // Get a specific order
            group.MapGet("/{orderId:guid}", async (Guid orderId, IOrderService orderService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var customerId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

                if (string.IsNullOrWhiteSpace(customerId))
                    return Results.Unauthorized();

                var order = await orderService.GetOrderAsync(orderId, customerId, cancellationToken);

                return order is null ? Results.NotFound() : Results.Ok(order);
            })
            .WithName("GetOrder")
            .WithSummary("Gets an individual customer order");


            // Get all orders belonging to the authenticated customer
            group.MapGet("/my-orders", async (IOrderService orderService, ClaimsPrincipal user, CancellationToken cancellationToken) =>
            {
                var customerId =
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub");

                if (string.IsNullOrWhiteSpace(customerId))
                    return Results.Unauthorized();

                var orders = await orderService.GetCustomerOrdersAsync(
                    customerId,
                    cancellationToken);

                return Results.Ok(orders);
            })
            .WithName("GetMyOrders")
            .WithSummary("Gets all orders belonging to the authenticated customer");

        }
            
    }
}
