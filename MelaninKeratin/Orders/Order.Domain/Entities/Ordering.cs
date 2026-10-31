using Order.Domain.Enums;

namespace Order.Domain.Entities
{
    public sealed class Ordering
    {
        private readonly List<OrderItem> _items = [];

        public Guid Id { get; private set; }

        public string CustomerId { get; private set; } = string.Empty;

        public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment;

        public string? PaymentReference { get; private set; }

        public decimal TotalAmount { get; private set; }

        public string Currency { get; private set; } = "NGN";

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? ConfirmedAtUtc { get; private set; }

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Ordering() { } // EF Core

        public Ordering(string customerId, IEnumerable<OrderItem> items)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.");

            var orderItems = items?.ToList()
                ?? throw new ArgumentNullException(nameof(items));

            if (orderItems.Count == 0)
                throw new InvalidOperationException(
                    "An order must contain at least one product.");

            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAtUtc = DateTime.UtcNow;

            _items.AddRange(orderItems);

            TotalAmount = _items.Sum(item => item.SubTotal);
        }

        public void SetPaymentReference(string reference)
        {
            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException(
                    "Only pending orders can receive a payment reference.");

            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException("Payment reference is required.");

            PaymentReference = reference;
        }

        public void ConfirmPayment(string reference)
        {
            if (Status == OrderStatus.Confirmed)
                return;

            if (Status != OrderStatus.PendingPayment)
                throw new InvalidOperationException(
                    "This order cannot be confirmed.");

            if (!string.Equals(
                PaymentReference,
                reference,
                StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "Payment reference does not match this order.");
            }

            Status = OrderStatus.Confirmed;
            ConfirmedAtUtc = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != OrderStatus.PendingPayment &&
                Status != OrderStatus.Confirmed)
            {
                throw new InvalidOperationException(
                    "This order cannot be cancelled.");
            }

            Status = OrderStatus.Cancelled;
        }
    }
}
