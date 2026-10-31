namespace Cart.Api.Models
{
    public class ShopCart
    {
        //public string ShopCartId { get; set; } = Guid.NewGuid().ToString(); 

        public string UserName { get; set; } = default!;
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public ShopCart(string userName)
        {
            UserName = userName;
        }

        public ShopCart()
        {

        }

    }
}
