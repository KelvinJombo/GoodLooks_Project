using Cart.Api.Models;
using Carter;
using Mapster;
using MediatR;

namespace Cart.Api.ShoppingCart.SaveCart
{

    public record SaveCartRequest(ShopCart Cart);
    public record SaveCartResponse(string UserName);
    public class SaveCartEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/shopCart", async (SaveCartRequest request, ISender sender) =>
            {
                var command = request.Adapt<SaveCartCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<SaveCartResponse>();

                return Results.Created($"/shopCart/{response.UserName}", response);
            })
                .WithName("Save ShoppingCart")
                .Produces<SaveCartResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Saving Cart Content")
                .WithDescription("Save Cart");
        }
    }
}
