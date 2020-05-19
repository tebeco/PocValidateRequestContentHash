using InOutLogging;

namespace Microsoft.AspNetCore.Builder
{
    public static class InOutLoggingApplicationBuilderExtentions
    {
        public static IApplicationBuilder UseInOutLogging(this IApplicationBuilder app) =>
            app.UseMiddleware<InOutLoggingMiddleware>();
    }
}
