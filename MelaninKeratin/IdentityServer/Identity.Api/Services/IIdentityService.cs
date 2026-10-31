using Identity.Api.Dtos;
using Microsoft.AspNetCore.Identity.Data;

namespace Identity.Api.Services
{
    public interface IIdentityService
    {
        Task<UserProfileResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<UserProfileResponseDto?> GetProfileAsync(string userId);

        Task<UserProfileResponseDto?> UpdateProfileAsync(string userId, UpdateProfileRequestDto request);

        Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequestDto request);
        Task RequestPasswordResetAsync(string email);

        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
