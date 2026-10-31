using Order.Domain.Enums;

namespace Order.Domain.Entities
{
    public sealed class Booking
    {
        public Guid Id { get; private set; }

        public string CustomerId { get; private set; } = string.Empty;

        public Guid TenantId { get; private set; }

        public Guid ProfessionalId { get; private set; }

        public Guid ServiceId { get; private set; }

        public string ServiceName { get; private set; } = string.Empty;

        public DateTime StartTimeUtc { get; private set; }

        public DateTime EndTimeUtc { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; } = "NGN";

        public string? PaymentReference { get; private set; }

        public BookingStatus Status { get; private set; } = BookingStatus.PendingPayment;

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? ConfirmedAtUtc { get; private set; }

        public DateTime? CancelledAtUtc { get; private set; }

        private Booking() { } // EF Core

        public Booking(
            string customerId,
            Guid tenantId,
            Guid professionalId,
            Guid serviceId,
            string serviceName,
            DateTime startTimeUtc,
            DateTime endTimeUtc,
            decimal amount,
            string paymentReference)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.");

            if (tenantId == Guid.Empty ||
                professionalId == Guid.Empty ||
                serviceId == Guid.Empty)
                throw new ArgumentException("Invalid booking identifiers.");

            if (endTimeUtc <= startTimeUtc)
                throw new ArgumentException("Invalid appointment time.");

            if (amount <= 0)
                throw new ArgumentException("Booking amount must be positive.");

            if (string.IsNullOrWhiteSpace(paymentReference))
                throw new ArgumentException("Payment reference is required.");

            Id = Guid.NewGuid();
            CustomerId = customerId;
            TenantId = tenantId;
            ProfessionalId = professionalId;
            ServiceId = serviceId;
            ServiceName = serviceName;
            StartTimeUtc = startTimeUtc;
            EndTimeUtc = endTimeUtc;
            Amount = amount;
            PaymentReference = paymentReference;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void ConfirmPayment()
        {
            if (Status == BookingStatus.Confirmed)
                return;

            if (Status != BookingStatus.PendingPayment)
                throw new InvalidOperationException(
                    "This booking cannot be confirmed.");

            Status = BookingStatus.Confirmed;
            ConfirmedAtUtc = DateTime.UtcNow;
        }

        public void Cancel(string requestingCustomerId)
        {
            if (!string.Equals(
                CustomerId,
                requestingCustomerId,
                StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException(
                    "Only the customer who created this booking can cancel it.");
            }

            if (Status != BookingStatus.PendingPayment &&
                Status != BookingStatus.Confirmed)
            {
                throw new InvalidOperationException(
                    "This booking cannot be cancelled.");

            }

            Status = BookingStatus.Cancelled;
            CancelledAtUtc = DateTime.UtcNow;
        }
    }
}
