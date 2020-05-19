namespace ContentHashValidation
{
    public class ContentHashValidationOptions
    {
        public const string DefaultHeaderName = "X-Content-Hash";
        public const string DefaultHashName = "SHA-256";

        public string HeaderName { get; set; } = DefaultHeaderName;

        public string HashName { get; set; } = DefaultHashName;

        public PoolKind PoolKind { get; set; } = PoolKind.DedicatedArrayPool;
    }
}
