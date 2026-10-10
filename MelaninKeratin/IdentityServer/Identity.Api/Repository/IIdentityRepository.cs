using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Repository
{
    public interface IIdentityRepository
    {
        Task<User?> FindByIdAsync(string userId);

        Task<User?> FindByEmailAsync(string email);

        Task<IdentityResult> CreateUserAsync(User user, string password);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    }
}
