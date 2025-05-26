namespace ContentHashValidation
{
    public struct ContentHashValidationResult
    {
        public static readonly ContentHashValidationResult Success = new ContentHashValidationResult(true);
        public static readonly ContentHashValidationResult Failure = new ContentHashValidationResult(false);

        public ContentHashValidationResult(bool succeed) : this()
        {
            Succeed = succeed;
        }

        public bool Succeed { get; }
    }
}
