using Cart.Api.Models;

namespace Cart.Api.Data
{
    public interface ICartRepository
    {
        Task<ShopCart> GetCart(string userName, CancellationToken cancellationToken);
        Task<ShopCart> StoreCart(ShopCart cart, CancellationToken cancellationToken);
        Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default);
    }
}
