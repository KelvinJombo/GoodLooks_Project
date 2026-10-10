namespace Identity.Api.Exceprions
{
    public sealed class IdentityOperationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public IdentityOperationException(
            string message,
            IEnumerable<string>? errors = null)
            : base(message)
        {
            Errors = errors ?? [];
        }
    }
}
