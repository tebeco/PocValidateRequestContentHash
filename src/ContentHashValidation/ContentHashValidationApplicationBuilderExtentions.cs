using MyWebApi.ContentHashValidation;

namespace Microsoft.AspNetCore.Builder
{
    public static class ContentHashValidationApplicationBuilderExtentions
    {
        public static IApplicationBuilder UseContentHashValidation(this IApplicationBuilder app) =>
            app.UseMiddleware<ContentHashValidationMiddleware>();
    }
}
