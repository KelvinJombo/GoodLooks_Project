using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public sealed class RegisterRequestDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; init; } = string.Empty;

        [Required]
        [MinLength(12)]
        [StringLength(128)]
        public string Password { get; init; } = string.Empty;
    }
}
