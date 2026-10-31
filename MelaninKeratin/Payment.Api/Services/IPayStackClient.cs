namespace Payment.Api.Services;

public sealed record PaystackInitialization(string Reference, string AuthorizationUrl, string AccessCode);

public sealed record PaystackVerification(string Reference, string Status, long Amount, string Currency, string? Channel, DateTimeOffset? PaidAt);

public interface IPaystackClient
{
    Task<PaystackInitialization> InitializeAsync(string email, long amountInMinorUnits, string currency, string reference, string callbackUrl, CancellationToken cancellationToken);

    Task<PaystackVerification> VerifyAsync(string reference, CancellationToken cancellationToken);
}