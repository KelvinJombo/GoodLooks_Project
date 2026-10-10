namespace Identity.Api.Services
{
    public interface IIdentityEmailService
    {
        Task SendPasswordResetAsync(string email, string resetToken);
    }
}
