using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Repository
{
    public sealed class IdentityRepository(
    UserManager<User> userManager) : IIdentityRepository
    {
        public Task<User?> FindByIdAsync(string userId) =>
            userManager.FindByIdAsync(userId);

        public Task<User?> FindByEmailAsync(string email) => userManager.FindByEmailAsync(email);

        public Task<IdentityResult> CreateUserAsync(User user, string password) => 
            userManager.CreateAsync(user, password);

        public Task<IdentityResult> UpdateUserAsync(User user) => userManager.UpdateAsync(user);

        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
            userManager.ChangePasswordAsync(
                user, currentPassword, newPassword);

        public Task<string> GeneratePasswordResetTokenAsync(User user) =>
            userManager.GeneratePasswordResetTokenAsync(user);

        public Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword) =>
            userManager.ResetPasswordAsync(user, token, newPassword);
    }
}
