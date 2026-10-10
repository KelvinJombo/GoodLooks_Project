using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public sealed class ResetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Token { get; init; } = string.Empty;

        [Required]
        [MinLength(12)]
        [StringLength(128)]
        public string NewPassword { get; init; } = string.Empty;
    }
}
