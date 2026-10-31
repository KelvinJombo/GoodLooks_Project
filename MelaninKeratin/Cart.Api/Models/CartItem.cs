namespace Cart.Api.Models
{
    public class CartItem
    {
        public int Quantity { get; set; } = default;
        public decimal Price { get; set; } = default;
        public Guid ProductId { get; set; } = default;
        public string ProductName { get; set; } = default;
    }
}
