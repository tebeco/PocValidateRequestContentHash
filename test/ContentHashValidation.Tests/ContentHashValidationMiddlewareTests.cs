using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyWebApi.ContentHashValidation;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentHashValidation.Tests
{
    public class ContentHashValidationMiddlewareTests
    {
        [Theory]
        [InlineData(true, 10)]
        [InlineData(false, 10)]
        [InlineData(true, 16384)]
        [InlineData(false, 16384)]
        public async Task Should_return_200_request_When_hash_match(bool isValidatedRoute, int payloadSize)
        {
            var buffer = Encoding.UTF8.GetBytes(new string('-', payloadSize));
            var hash = SHA256.Create().ComputeHash(buffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(buffer); //16Ko
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            httpContext.Request.Method = "POST";
            httpContext.Request.Path = isValidatedRoute ? "/validationcontent/validated" : "/";

            var next = new RequestDelegate(context => { context.Response.StatusCode = 200; return Task.CompletedTask; });
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(16384)]
        public async Task Should_return_400_request_When_hash_match(int payloadSize)
        {
            var buffer = Encoding.UTF8.GetBytes(new string('-', payloadSize));
            var hash = SHA256.Create().ComputeHash(buffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(buffer); //16Ko
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = new string('0', 64);
            httpContext.Request.Method = "POST";
            httpContext.Request.Path = "/validationcontent/validated";

            var next = new RequestDelegate(_ => Task.CompletedTask);
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
        }
    }
}
