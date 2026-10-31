using Cart.Api.Models;
using Carter;
using Mapster;
using MediatR;

namespace Cart.Api.ShoppingCart.GetCart
{
    //public record GetCartRequest(string UserName);
    public record GetCartResponse(ShopCart Cart);
    public class GetCartEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/shopCart/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetCartQuery(userName));
                var response = result.Adapt<GetCartResponse>();
                return Results.Ok(response);
            })
                .WithName("GetCartByUserName")
                .Produces<GetCartResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Cart By UserName")
                .WithDescription("GetCart By Id");
        }
    }
}
