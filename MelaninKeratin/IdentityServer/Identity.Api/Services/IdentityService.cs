using Identity.Api.Dtos;
using Identity.Api.Exceprions;
using Identity.Api.Models;
using Identity.Api.Repository;
using Microsoft.AspNetCore.Identity.Data;

namespace Identity.Api.Services
{
    public sealed class IdentityService(IIdentityRepository repository, IIdentityEmailService emailService) : IIdentityService
    {
        public async Task<UserProfileResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser =
                await repository.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                throw new IdentityOperationException(
                    "An account with this email already exists.");
            }

            var now = DateTime.UtcNow;

            var user = new User
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim(),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                CreatedAt = now,
                UpdatedAt = now
            };

            var result = await repository.CreateUserAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new IdentityOperationException("Registration failed.", result.Errors.Select(e => e.Description));
            }

            // Do not automatically assign Admin or other privileged roles.
            return ToProfile(user);
        }

        public async Task<UserProfileResponseDto?> GetProfileAsync(string userId)
        {
            var user = await repository.FindByIdAsync(userId);

            return user is null ? null : ToProfile(user);
        }

        public async Task<UserProfileResponseDto?> UpdateProfileAsync(string userId, UpdateProfileRequestDto request)
        {
            var user = await repository.FindByIdAsync(userId);

            if (user is null)
                return null;

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.UpdatedAt = DateTime.UtcNow;

            var result = await repository.UpdateUserAsync(user);

            EnsureSucceeded(result);

            return ToProfile(user);
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequestDto request)
        {
            var user = await repository.FindByIdAsync(userId);

            if (user is null)
                return false;

            var result = await repository.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            EnsureSucceeded(result);

            return true;
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await repository.FindByEmailAsync(email);

            // Avoid revealing whether the email is registered.
            if (user is null || !user.EmailConfirmed)
                return;

            var token =
                await repository.GeneratePasswordResetTokenAsync(user);

            await emailService.SendPasswordResetAsync(
                user.Email!,
                token);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await repository.FindByEmailAsync(request.Email);

            if (user is null)
                return false;

            var result = await repository.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword);

            EnsureSucceeded(result);

            return true;
        }

        private static UserProfileResponseDto ToProfile(User user) =>
            new(
                user.Id,
                user.Email ?? string.Empty,
                user.FirstName,
                user.LastName);

        private static void EnsureSucceeded(
            Microsoft.AspNetCore.Identity.IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new IdentityOperationException(
                    "The requested identity operation failed.",
                    result.Errors.Select(e => e.Description));
            }
        }
    }
}
