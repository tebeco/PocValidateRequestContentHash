using ContentHashValidation;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContentHashValidationServiceCollectionExtentions
    {
        public static IServiceCollection AddContentHashValidation(this IServiceCollection services)
        {
            services
                .AddOptionsWithValidateOnStart<ContentHashValidationOptions>()
                .Validate(options => !string.IsNullOrWhiteSpace(options.HeaderName), $"{nameof(ContentHashValidationOptions)}:{nameof(ContentHashValidationOptions.HeaderName)} cannot be null or empty.")
                .Validate(options => !string.IsNullOrWhiteSpace(options.HashName) && CryptoConfig.CreateFromName(options.HashName) is HashAlgorithm, $"{nameof(ContentHashValidationOptions)}:{nameof(ContentHashValidationOptions.HeaderName)} must be a valid HashName.")
                ;

            return services;
        }

        public static IServiceCollection AddContentHashValidation(this IServiceCollection services, Action<ContentHashValidationOptions> configure) =>
            services.AddContentHashValidation()
                    .Configure(configure);
    }
}
