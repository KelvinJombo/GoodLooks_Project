using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public sealed class ChangePasswordRequestDto
    {
        [Required]
        public string CurrentPassword { get; init; } = string.Empty;

        [Required]
        [MinLength(12)]
        [StringLength(128)]
        public string NewPassword { get; init; } = string.Empty;
    }
}
