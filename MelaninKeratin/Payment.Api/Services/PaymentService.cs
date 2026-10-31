using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payment.Api.Data;
using Payment.Api.Models;
using Payment.Api.Options;

namespace Payment.Api.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly PaymentDbContext _db;
    private readonly IPaystackClient _paystack;
    //private readonly IOrderPaymentLookup _orders;
    private readonly PaystackOptions _options;

    public PaymentService(PaymentDbContext db, IPaystackClient paystack,/* IOrderPaymentLookup orders*/ IOptions<PaystackOptions> options)
    {
        _db = db;
        _paystack = paystack;
        //_orders = orders;
        _options = options.Value;
    }

    //public async Task<PaymentCheckoutResult> InitializeAsync(string orderId, string customerId, CancellationToken cancellationToken)
    //{
    //    var order = await _orders.GetPayableOrderAsync(orderId, customerId, cancellationToken);

    //    if (order is null || !order.IsPayable)
    //    {
    //        throw new InvalidOperationException("The order does not exist, does not belong to this customer, or is not payable.");
    //    }

    //    if (order.Amount <= 0)
    //    {
    //        throw new InvalidOperationException("The order amount must be greater than zero.");
    //    }

    //    if (!string.Equals(order.Currency, _options.Currency, StringComparison.OrdinalIgnoreCase))
    //    {
    //        throw new InvalidOperationException("The order currency is not supported by this payment configuration.");
    //    }

    //    var amountInMinorUnits = checked((long)decimal.Round(order.Amount * 100m, 0, MidpointRounding.AwayFromZero));

    //    var reference = $"MK-{Guid.NewGuid():N}";

    //    var payment = new PaymentInfo
    //    {
    //        Id = Guid.NewGuid(),
    //        OrderId = order.OrderId,
    //        CustomerId = customerId,
    //        Reference = reference,
    //        Amount = order.Amount,
    //        Currency = order.Currency.ToUpperInvariant(),
    //        Status = PaymentStatus.Pending,
    //        CreatedAtUtc = DateTime.UtcNow
    //    };

    //    _db.Payments.Add(payment);
    //    await _db.SaveChangesAsync(cancellationToken);

    //    try
    //    {
    //        var checkout = await _paystack.InitializeAsync(order.CustomerEmail, amountInMinorUnits, payment.Currency, reference, _options.CallbackUrl, cancellationToken);

    //        payment.AuthorizationUrl = checkout.AuthorizationUrl;
    //        payment.AccessCode = checkout.AccessCode;
    //        payment.UpdatedAtUtc = DateTime.UtcNow;

    //        await _db.SaveChangesAsync(cancellationToken);

    //        return new PaymentCheckoutResult(payment.Id, payment.Reference, payment.AuthorizationUrl);
    //    }
    //    catch
    //    {
    //        // Keep the pending record for troubleshooting.
    //        // A timeout does not prove that Paystack failed
    //        // to initialize the transaction.
    //        throw;
    //    }
    //}

    public async Task<PaystackVerification> VerifyAsync( string reference, string customerId, CancellationToken cancellationToken)
    {
        var payment = await _db.Payments
            .SingleOrDefaultAsync(
                p => p.Reference == reference
                     && p.CustomerId == customerId,
                cancellationToken);

        if (payment is null)
        {
            throw new KeyNotFoundException("Payment was not found for this customer.");
        }

        var verified = await _paystack.VerifyAsync(payment.Reference, cancellationToken);

        if (!string.Equals(verified.Reference,payment.Reference, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "The verified reference does not match the local payment.");
        }

        if (!string.Equals(
            verified.Currency,
            payment.Currency,
            StringComparison.OrdinalIgnoreCase)
            || verified.Amount != checked(
                (long)decimal.Round(
                    payment.Amount * 100m,
                    0,
                    MidpointRounding.AwayFromZero)))
        {
            throw new InvalidOperationException(
                "The verified amount or currency does not match the local payment.");
        }

        if (verified.Status == "success")
        {
            if (payment.Status != PaymentStatus.Successful)
            {
                payment.Status = PaymentStatus.Successful;
                payment.PaidAtUtc = verified.PaidAt?.UtcDateTime
                    ?? DateTime.UtcNow;
                payment.Channel = verified.Channel;
                payment.UpdatedAtUtc = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);

                // TODO: notify the Order service to mark the
                // order as paid, using an idempotent operation.
            }
        }
        else if (verified.Status == "failed")
        {
            if (payment.Status == PaymentStatus.Pending)
            {
                payment.Status = PaymentStatus.Failed;
                payment.UpdatedAtUtc = DateTime.UtcNow;

                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        return verified;
    }


    public async Task ProcessSuccessfulWebhookAsync(string reference, CancellationToken cancellationToken)
    {
        var payment = await _db.Payments.SingleOrDefaultAsync(p => p.Reference == reference, cancellationToken);

        if (payment is null)
        {
            throw new KeyNotFoundException(
                "No local payment matches the webhook reference.");
        }

        // A duplicate notification must not process the payment twice.
        if (payment.Status == PaymentStatus.Successful)
        {
            return;
        }

        var verified = await _paystack.VerifyAsync(reference, cancellationToken);

        var expectedAmount = checked((long)decimal.Round(payment.Amount * 100m, 0, MidpointRounding.AwayFromZero));

        if (!string.Equals(
                verified.Reference,
                payment.Reference,
                StringComparison.Ordinal)
            || !string.Equals(
                verified.Status,
                "success",
                StringComparison.OrdinalIgnoreCase)
            || verified.Amount != expectedAmount
            || !string.Equals(
                verified.Currency,
                payment.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Paystack verification did not match the local payment.");
        }

        payment.Status = PaymentStatus.Successful;
        payment.PaidAtUtc = verified.PaidAt?.UtcDateTime
            ?? DateTime.UtcNow;
        payment.Channel = verified.Channel;
        payment.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        // TODO: Notify the Order service through an idempotent
        // operation or a durable outbox event.
    }


}