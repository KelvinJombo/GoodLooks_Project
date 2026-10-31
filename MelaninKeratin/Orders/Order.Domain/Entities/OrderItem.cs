namespace Order.Domain.Entities
{
    public sealed class OrderItem
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; } = string.Empty;

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal SubTotal => Quantity * UnitPrice;

        private OrderItem() { }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID is required.");

            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name is required.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");

            if (unitPrice <= 0)
                throw new ArgumentException("Price must be positive.");

            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
