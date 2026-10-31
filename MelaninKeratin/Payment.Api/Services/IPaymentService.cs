namespace Payment.Api.Services;

public sealed record PaymentCheckoutResult(Guid PaymentId, string Reference, string AuthorizationUrl);

public interface IPaymentService
{
   // Task<PaymentCheckoutResult> InitializeAsync(string orderId, string customerId, CancellationToken cancellationToken);

    Task<PaystackVerification> VerifyAsync(string reference, string customerId, CancellationToken cancellationToken);

    Task ProcessSuccessfulWebhookAsync(string reference,CancellationToken cancellationToken);
}