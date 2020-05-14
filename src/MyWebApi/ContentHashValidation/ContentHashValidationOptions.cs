namespace MyWebApi.ContentHashValidation
{
    public class ContentHashValidationOptions
    {
        public string HeaderName { get; set; } = "X-ContentHash-Validation";

        public string HashName { get; set; } = "SHA-256";
    }
}
