using Identity.Api.Dtos;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Api.Services
{
    public sealed class ExternalLoginService : IExternalLoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ExternalLoginService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ExternalLoginResult> CompleteAsync(
            ExternalLoginInfo externalLoginInfo)
        {
            var provider = externalLoginInfo.LoginProvider;
            var providerKey = externalLoginInfo.ProviderKey;

            // First, try to sign in an account already linked
            // to this provider and provider-specific user ID.
            var signInResult = await _signInManager
                .ExternalLoginSignInAsync(
                    provider,
                    providerKey,
                    isPersistent: false);

            if (signInResult.Succeeded)
            {
                return new(ExternalLoginStatus.SignedIn);
            }

            if (signInResult.IsLockedOut)
            {
                return new(ExternalLoginStatus.LockedOut);
            }

            if (signInResult.IsNotAllowed)
            {
                return new(ExternalLoginStatus.NotAllowed);
            }

            if (signInResult.RequiresTwoFactor)
            {
                return new(ExternalLoginStatus.RequiresTwoFactor);
            }

            // This provider identity is not linked to a local account.
            var email = externalLoginInfo.Principal
                .FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return new(ExternalLoginStatus.EmailMissing);
            }

            email = email.Trim();

            // Never silently link an external identity to an
            // existing account just because its email matches.
            var existingUser =
                await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new(ExternalLoginStatus.EmailAlreadyRegistered);
            }

            var firstName = externalLoginInfo.Principal
                .FindFirstValue(ClaimTypes.GivenName);

            var lastName = externalLoginInfo.Principal
                .FindFirstValue(ClaimTypes.Surname);

            var now = DateTime.UtcNow;

            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = now,
                UpdatedAt = now
            };

            // Create an account without a local password.
            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                return new(
                    ExternalLoginStatus.Failed,
                    createResult.Errors.Select(e => e.Code).ToArray());
            }

            // Persist the association using the provider's stable ID.
            var addLoginResult = await _userManager.AddLoginAsync(
                user,
                new UserLoginInfo(provider, providerKey, provider));

            if (!addLoginResult.Succeeded)
            {
                // Avoid leaving an orphaned account behind.
                await _userManager.DeleteAsync(user);

                return new(
                    ExternalLoginStatus.Failed,
                    addLoginResult.Errors.Select(e => e.Code).ToArray());
            }

            // Establish the application's normal local session.
            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            return new(ExternalLoginStatus.Created);
        }
    }

}