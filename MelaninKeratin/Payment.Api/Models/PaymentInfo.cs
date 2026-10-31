namespace Payment.Api.Models
{
    public sealed class PaymentInfo
    {
        public Guid Id { get; set; }

        public string OrderId { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string Provider { get; set; } = "Paystack";

        public string Reference { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "NGN";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? AuthorizationUrl { get; set; }

        public string? AccessCode { get; set; }

        public string? Channel { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }
    }
}
