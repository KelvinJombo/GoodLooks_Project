namespace Identity.Api.Dtos
{
    public sealed record UserProfileResponseDto(
    string Id,
    string Email,
    string? FirstName,
    string? LastName);
}
