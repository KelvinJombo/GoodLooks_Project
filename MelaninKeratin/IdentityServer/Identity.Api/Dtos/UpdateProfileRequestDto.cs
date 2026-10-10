using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Dtos
{
    public sealed class UpdateProfileRequestDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; init; } = string.Empty;
    }
}
