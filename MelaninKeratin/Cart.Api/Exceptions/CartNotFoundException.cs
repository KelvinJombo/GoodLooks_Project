namespace Cart.Api.Exceptions
{
    public sealed class CartNotFoundException : Exception
    {
        public CartNotFoundException(string userName) : base($"Shopping cart for user '{userName}' was not found.")
        {
        }
    }
}
