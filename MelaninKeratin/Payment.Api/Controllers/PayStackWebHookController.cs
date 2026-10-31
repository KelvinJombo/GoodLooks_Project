using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payment.Api.Options;
using Payment.Api.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Payment.Api.Controllers;

[ApiController]
[Route("api/payments/webhook")]
public sealed class PaystackWebhookController : ControllerBase
{
    private readonly PaystackOptions _options;
    private readonly IPaymentService _payments;
    private readonly ILogger<PaystackWebhookController> _logger;

    public PaystackWebhookController(IOptions<PaystackOptions> options, IPaymentService payments, ILogger<PaystackWebhookController> logger)
    {
        _options = options.Value;
        _payments = payments;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Receive(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync(cancellationToken);

        var signature = Request.Headers["x-paystack-signature"].ToString();

        if (string.IsNullOrWhiteSpace(signature)
            || !IsValidSignature(rawBody, signature))
        {
            return Unauthorized();
        }

        using var document = JsonDocument.Parse(rawBody);
        var root = document.RootElement;

        var eventName = root.TryGetProperty(
            "event", out var eventElement)
            ? eventElement.GetString()
            : null;

        if (eventName != "charge.success")
        {
            return Ok();
        }

        var data = root.GetProperty("data");

        var reference = data.TryGetProperty("reference", out var referenceElement) ? referenceElement.GetString() : null; 

        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest();
        }

        try
        {
            // Implement this method to find the local payment
            // by reference, verify it with Paystack, compare
            // amount/currency, and update it idempotently.
            await _payments.ProcessSuccessfulWebhookAsync(reference, cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process Paystack webhook for reference {Reference}",
                reference);

            // A non-2xx response allows Paystack to retry.
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private bool IsValidSignature(
        string rawBody,
        string suppliedSignature)
    {
        var key = Encoding.UTF8.GetBytes(_options.SecretKey);
        var body = Encoding.UTF8.GetBytes(rawBody);

        var hash = HMACSHA512.HashData(key, body);
        var expectedSignature = Convert.ToHexString(hash)
            .ToLowerInvariant();

        try
        {
            return CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(expectedSignature),
                Encoding.ASCII.GetBytes(suppliedSignature));
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}