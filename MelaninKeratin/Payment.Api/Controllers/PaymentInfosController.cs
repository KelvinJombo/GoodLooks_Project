using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.DTOs;
using Payment.Api.Services;

namespace Payment.Api.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public sealed class PaymentsController : ControllerBase
{
    private readonly IPaymentService _payments;

    public PaymentsController(IPaymentService payments)
    {
        _payments = payments;
    }

    [HttpPost("initialize")]
    //public async Task<IActionResult> Initialize([FromBody] InitializePaymentRequest request, CancellationToken cancellationToken)
    //{
    //    var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

    //    if (string.IsNullOrWhiteSpace(customerId))
    //    {
    //        return Unauthorized();
    //    }

    //    var result = await _payments.InitializeAsync( request.OrderId, customerId, cancellationToken);

    //    return Ok(result);
    //}

    [HttpGet("verify/{reference}")]
    public async Task<IActionResult> Verify(string reference, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(customerId))
        {
            return Unauthorized();
        }

        var result = await _payments.VerifyAsync(
            reference,
            customerId,
            cancellationToken);

        return Ok(result);
    }
}