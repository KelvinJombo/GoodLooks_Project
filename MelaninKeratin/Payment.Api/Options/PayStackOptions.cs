namespace Payment.Api.Options
{
    public sealed class PaystackOptions
    {
        public const string SectionName = "Paystack";

        public string SecretKey { get; set; } = string.Empty;

        public string CallbackUrl { get; set; } = "https://localhost:22167";

        public string Currency { get; set; } = "NGN";
    }
}
