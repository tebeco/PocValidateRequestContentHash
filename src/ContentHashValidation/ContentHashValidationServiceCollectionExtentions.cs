using ContentHashValidation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContentHashValidationServiceCollectionExtentions
    {
        public static IServiceCollection AddContentHashValidation(this IServiceCollection services)
        {
            services.AddOptions<ContentHashValidationOptions>();

            return services;
        }

        public static IServiceCollection AddContentHashValidation(this IServiceCollection services, Action<ContentHashValidationOptions> configure) =>
            services.AddContentHashValidation()
                    .Configure(configure);
    }
}
