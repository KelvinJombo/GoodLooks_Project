namespace Identity.Api.Dtos
{
    public enum ExternalLoginStatus
    {
        SignedIn,
        Created,
        EmailMissing,
        EmailAlreadyRegistered,
        LockedOut,
        NotAllowed,
        RequiresTwoFactor,
        Failed
    }

    public sealed record ExternalLoginResult(
        ExternalLoginStatus Status,
        IReadOnlyList<string>? Errors = null);
}
