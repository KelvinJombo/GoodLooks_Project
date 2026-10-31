using Duende.IdentityServer.Services;
using Identity.Api.Dtos;
using Identity.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ExternalLoginController : Controller
    {
        private readonly SignInManager<Models.User> _signInManager;
        private readonly IExternalLoginService _externalLoginService;
        private readonly IIdentityServerInteractionService _interaction;

        public ExternalLoginController(
            SignInManager<Models.User> signInManager,
            IExternalLoginService externalLoginService,
            IIdentityServerInteractionService interaction)
        {
            _signInManager = signInManager;
            _externalLoginService = externalLoginService;
            _interaction = interaction;
        }

        [HttpGet("login")]
        public IActionResult Login(
            [FromQuery] string provider,
            [FromQuery] string? returnUrl)
        {
            if (provider is not ("Google" or "Facebook"))
            {
                return BadRequest("Unsupported external provider.");
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                !_interaction.IsValidReturnUrl(returnUrl) &&
                !Url.IsLocalUrl(returnUrl))
            {
                return BadRequest("Invalid return URL.");
            }

            var callbackUrl = Url.Action(
                nameof(Callback),
                "ExternalLogin",
                new { returnUrl });

            if (callbackUrl is null)
            {
                return Problem("Unable to create the callback URL.");
            }

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    provider,
                    callbackUrl);

            return Challenge(properties, provider);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            [FromQuery] string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                !_interaction.IsValidReturnUrl(returnUrl) &&
                !Url.IsLocalUrl(returnUrl))
            {
                return BadRequest("Invalid return URL.");
            }

            var externalInfo =
                await _signInManager.GetExternalLoginInfoAsync();

            if (externalInfo is null)
            {
                return Unauthorized(
                    "External authentication information is unavailable.");
            }

            var result = await _externalLoginService
                .CompleteAsync(externalInfo);

            // The temporary external cookie is no longer needed.
            await HttpContext.SignOutAsync(
                IdentityConstants.ExternalScheme);

            switch (result.Status)
            {
                case ExternalLoginStatus.SignedIn:
                case ExternalLoginStatus.Created:
                    return LocalRedirect(
                        !string.IsNullOrWhiteSpace(returnUrl)
                            ? returnUrl
                            : "/");

                case ExternalLoginStatus.EmailMissing:
                    return Conflict(
                        "The provider did not return an email address. " +
                        "Please use another registration method.");

                case ExternalLoginStatus.EmailAlreadyRegistered:
                    return Conflict(
                        "An account with this email already exists. " +
                        "Sign in to that account before linking a provider.");

                case ExternalLoginStatus.LockedOut:
                    return StatusCode(
                        StatusCodes.Status423Locked,
                        "This account is locked.");

                case ExternalLoginStatus.NotAllowed:
                    return Forbid();

                case ExternalLoginStatus.RequiresTwoFactor:
                    return Conflict(
                        "Additional two-factor authentication is required.");

                default:
                    // Log detailed error codes server-side.
                    // Do not expose Identity internals to users.
                    return Problem(
                        "External sign-in could not be completed.");
            }
        }
    }
}
