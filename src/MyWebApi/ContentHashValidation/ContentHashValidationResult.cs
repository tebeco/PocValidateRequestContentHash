namespace MyWebApi.ContentHashValidation
{
    public struct ContentHashValidationResult
    {
        public static ContentHashValidationResult Success = new ContentHashValidationResult(true);
        public static ContentHashValidationResult Failure = new ContentHashValidationResult(false);

        public ContentHashValidationResult(bool succeed) : this()
        {
            Succeed = succeed;
        }

        public bool Succeed { get; }
    }
}
