using Identity.Api.Dtos;
using Identity.Api.Exceprions;
using Identity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class IdentityController(IIdentityService identityService) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            try
            {
                var profile =
                    await identityService.RegisterAsync(request);

                ///return CreatedAtAction(nameof(GetMyProfile), profile);
                return StatusCode(StatusCodes.Status201Created, profile);
            }
            catch (IdentityOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    errors = ex.Errors
                });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();

            if (userId is null)
                return Unauthorized();

            var profile =
                await identityService.GetProfileAsync(userId);

            return profile is null
                ? NotFound()
                : Ok(profile);
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile(UpdateProfileRequestDto request)
        {
            var userId = GetCurrentUserId();

            if (userId is null)
                return Unauthorized();

            try
            {
                var profile = await identityService.UpdateProfileAsync(userId, request);

                return profile is null
                    ? NotFound()
                    : Ok(profile);
            }
            catch (IdentityOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    errors = ex.Errors
                });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
        {
            var userId = GetCurrentUserId();

            if (userId is null)
                return Unauthorized();

            try
            {
                var changed = await identityService.ChangePasswordAsync(userId, request);

                return changed
                    ? NoContent()
                    : NotFound();
            }
            catch (IdentityOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    errors = ex.Errors
                });
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await identityService.RequestPasswordResetAsync(
                request.Email);

            // Same response whether or not the account exists.
            return Ok(new
            {
                message = "If an eligible account exists, password reset instructions will be sent."
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
        {
            try
            {
                var reset = await identityService.ResetPasswordAsync(request);

                return reset
                    ? NoContent()
                    : BadRequest(new
                    {
                        message = "The reset request could not be completed."
                    });
            }
            catch (IdentityOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    errors = ex.Errors
                });
            }
        }

        private string? GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
    }
}
