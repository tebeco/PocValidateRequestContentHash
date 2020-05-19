using InOutLogging;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InOutLoggingServiceCollectionExtentions
    {
        public static IServiceCollection AddInOutLogging(this IServiceCollection services)
        {
            services.AddOptions<InOutLoggingOptions>();

            return services;
        }
        public static IServiceCollection AddInOutLogging(this IServiceCollection services, Action<InOutLoggingOptions> configure) =>
            services.AddInOutLogging()
                    .Configure(configure);
    }
}