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
            httpContext.Request.Body = new MemoryStream(buffer);
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            httpContext.Request.Method = "POST";

            if (isValidatedRoute)
            {
                httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask,
                                                     new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                     "someRoute"));
            }

            var next = new RequestDelegate(context => { context.Response.StatusCode = 200; return Task.CompletedTask; });
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
        }

        [Theory]
        [InlineData(200, 10)]
        [InlineData(302, 10)]
        [InlineData(400, 10)]
        [InlineData(404, 10)]
        [InlineData(500, 10)]
        [InlineData(503, 10)]
        public async Task Should_not_change_existing_StatusCode_When_hash_match(int statusCode, int payloadSize)
        {
            var buffer = Encoding.UTF8.GetBytes(new string('-', payloadSize));
            var hash = SHA256.Create().ComputeHash(buffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(buffer);
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            httpContext.Request.Method = "POST";
            httpContext.SetEndpoint(new Endpoint(c => { c.Response.StatusCode = statusCode; return Task.CompletedTask; },
                                                 new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                 "someRoute"));

            var next = new RequestDelegate(_ => Task.CompletedTask);
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Should_return_400_request_When_hash_match()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = new string('0', 64);
            httpContext.Request.Method = "POST";
            httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask,
                                                 new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                 "someRoute"));

            var next = new RequestDelegate(_ => Task.CompletedTask);
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Should_return_400_request_When_missing_header()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            httpContext.Request.Method = "POST";
            httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask,
                                                 new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                 "someRoute"));

            var next = new RequestDelegate(_ => Task.CompletedTask);
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Should_return_400_request_When_wrong_hash_size()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = new string('0', 3);
            httpContext.Request.Method = "POST";
            httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask,
                                                 new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                 "someRoute"));

            var next = new RequestDelegate(_ => Task.CompletedTask);
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
        }

        [Theory]
        [InlineData(10, PoolKind.SharedArrayPool)]
        [InlineData(10, PoolKind.DedicatedArrayPool)]
        [InlineData(10, PoolKind.FixedLengthLockFree)]
        [InlineData(10, PoolKind.FixedLengthWithLock)]
        [InlineData(10, PoolKind.PreAllocatedFixedLengthLockFree)]
        [InlineData(10, PoolKind.PreAllocatedFixedLengthWithLock)]
        [InlineData(16384, PoolKind.SharedArrayPool)]
        [InlineData(16384, PoolKind.DedicatedArrayPool)]
        [InlineData(16384, PoolKind.FixedLengthLockFree)]
        [InlineData(16384, PoolKind.FixedLengthWithLock)]
        [InlineData(16384, PoolKind.PreAllocatedFixedLengthLockFree)]
        [InlineData(16384, PoolKind.PreAllocatedFixedLengthWithLock)]

        public async Task Should_return_200_request_When_hash_match_with_specific_pool(int payloadSize, PoolKind poolKind)
        {
            var buffer = Encoding.UTF8.GetBytes(new string('-', payloadSize));
            var hash = SHA256.Create().ComputeHash(buffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = new MemoryStream(buffer);
            httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            httpContext.Request.Method = "POST";

            httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask,
                                                 new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                 "someRoute"));

            var next = new RequestDelegate(context => { context.Response.StatusCode = 200; return Task.CompletedTask; });
            var middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions() { PoolKind = poolKind }));

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
        }
    }
}
