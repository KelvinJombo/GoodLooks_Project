namespace Identity.Api.Services
{
    public sealed class DevelopmentIdentityEmailService : IIdentityEmailService
    {
        public Task SendPasswordResetAsync(
            string email,
            string resetToken)
        {
            // Development placeholder only.
            // Do not use this implementation in production.
            Console.WriteLine(
                $"Password reset requested for {email}. " +
                "Configure a real email provider to deliver the reset link.");

            return Task.CompletedTask;
        }
    }
}
