using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Api.Pages.Account;

public sealed class LoginModel : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;

    public LoginModel(
        IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
    }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public bool HasValidReturnUrl =>
        !string.IsNullOrWhiteSpace(ReturnUrl) &&
        (_interaction.IsValidReturnUrl(ReturnUrl) ||
         Url.IsLocalUrl(ReturnUrl));

    public string GetProviderUrl(string provider)
    {
        if (provider is not ("Google" or "Facebook"))
        {
            throw new ArgumentException(
                "Unsupported provider.", nameof(provider));
        }

        if (!string.IsNullOrWhiteSpace(ReturnUrl) &&
            !HasValidReturnUrl)
        {
            throw new InvalidOperationException(
                "The return URL is invalid.");
        }

        return Url.Page(
            "/Account/Login",
            new { returnUrl = ReturnUrl }) is not null
            ? Url.Action(
                "Login",
                "ExternalLogin",
                new { provider, returnUrl = ReturnUrl })!
            : "/";
    }
}