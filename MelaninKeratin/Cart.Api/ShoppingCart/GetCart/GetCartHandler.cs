using Cart.Api.CQRS;
using Cart.Api.Data;
using Cart.Api.Models;

namespace Cart.Api.ShoppingCart.GetCart
{
    public record GetCartQuery(string UserName) : IQuery<GetCartResult>;

    public record GetCartResult(ShopCart Cart);

    public class GetCartQuerHandler(ICartRepository repository) : IQueryHandler<GetCartQuery, GetCartResult>
    {
        public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            var cart = await repository.GetCart(query.UserName, cancellationToken);

            return new GetCartResult(cart);
        }
    }
}
