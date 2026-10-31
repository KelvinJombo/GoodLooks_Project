using Carter;
using Mapster;
using MediatR;

namespace Cart.Api.ShoppingCart.DeleteCart
{
    public record DeleteCartRequest(string UserName);
    public record DeleteCartResponse(bool  IsSuccess);

    public class DeleteCartEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/shopCart/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteCartCommand(userName));

                var response = result.Adapt<DeleteCartResponse>();

                return Results.Ok(response);
            })
                .WithName("Delete Cart")
                .Produces<DeleteCartResponse>(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Cart")
                .WithDescription("Delete Cart");
        }
    }
}
