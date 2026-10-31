using Microsoft.Extensions.Options;
using Payment.Api.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Payment.Api.Services;

public sealed class PaystackClient : IPaystackClient
{
    private readonly HttpClient _httpClient;
    private readonly PaystackOptions _options;

    public PaystackClient(HttpClient httpClient, IOptions<PaystackOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.SecretKey);
    }

    public async Task<PaystackInitialization> InitializeAsync(
        string email,
        long amountInMinorUnits,
        string currency,
        string reference,
        string callbackUrl,
        CancellationToken cancellationToken)
    {
        var payload = new
        {
            email,
            amount = amountInMinorUnits.ToString(),
            currency,
            reference,
            callback_url = callbackUrl
        };

        using var response = await _httpClient.PostAsJsonAsync("transaction/initialize", payload, cancellationToken);

        var json = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Paystack initialization failed with HTTP {(int)response.StatusCode}.");
        }

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (!root.GetProperty("status").GetBoolean())
        {
            throw new InvalidOperationException("Paystack did not initialize the transaction.");
        }

        var data = root.GetProperty("data");

        return new PaystackInitialization(
            data.GetProperty("reference").GetString()!,
            data.GetProperty("authorization_url").GetString()!,
            data.GetProperty("access_code").GetString()!);
    }

    public async Task<PaystackVerification> VerifyAsync(string reference, CancellationToken cancellationToken)
    {
        var encodedReference = Uri.EscapeDataString(reference);

        using var response = await _httpClient.GetAsync($"transaction/verify/{encodedReference}", cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Paystack verification failed with HTTP {(int)response.StatusCode}.");
        }

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        if (!root.GetProperty("status").GetBoolean())
        {
            throw new InvalidOperationException("Paystack could not verify the transaction.");
        }

        var data = root.GetProperty("data");

        DateTimeOffset? paidAt = null;

        if (data.TryGetProperty("paid_at", out var paidAtElement)
            && paidAtElement.ValueKind == JsonValueKind.String
            && DateTimeOffset.TryParse(
                paidAtElement.GetString(),
                out var parsedPaidAt))
        {
            paidAt = parsedPaidAt;
        }

        string? channel = null;

        if (data.TryGetProperty("channel", out var channelElement)
            && channelElement.ValueKind == JsonValueKind.String)
        {
            channel = channelElement.GetString();
        }

        return new PaystackVerification(
            data.GetProperty("reference").GetString()!,
            data.GetProperty("status").GetString()!,
            data.GetProperty("amount").GetInt64(),
            data.GetProperty("currency").GetString()!,
            channel,
            paidAt);
    }
}