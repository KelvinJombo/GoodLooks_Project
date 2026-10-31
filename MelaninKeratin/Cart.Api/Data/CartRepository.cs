using Cart.Api.Exceptions;
using Cart.Api.Models;
using Marten;

namespace Cart.Api.Data
{
    public class CartRepository(IDocumentSession session) : ICartRepository
    {
        public async Task<ShopCart> GetCart(string userName, CancellationToken cancellationToken)
        {
            var cart = await session.LoadAsync<ShopCart>(userName, cancellationToken);

            return cart is null ? throw new CartNotFoundException(userName) : cart;
        }

        public async Task<ShopCart> StoreCart(ShopCart cart, CancellationToken cancellationToken)
        {
            session.Store(cart);

            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShopCart>(userName);

            await session.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
