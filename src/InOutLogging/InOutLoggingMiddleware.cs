using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InOutLogging
{
    public class InOutLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InOutLoggingMiddleware> _logger;
        private readonly InOutLoggingOptions _options;

        public InOutLoggingMiddleware(RequestDelegate next, IOptions<InOutLoggingOptions> options, ILogger<InOutLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
        }
    }
}
