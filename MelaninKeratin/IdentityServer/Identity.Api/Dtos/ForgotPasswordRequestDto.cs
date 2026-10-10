using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public sealed class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
    }
}
