using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MyWebApi.ContentHashValidation;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    [MemoryDiagnoser]
    public class PoolBenchmarks
    {
        private DefaultHttpContext _httpContext;
        private ContentHashValidationMiddleware _middleware;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var next = new RequestDelegate(_ => Task.CompletedTask);
            _middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions() { PoolKind = PoolKind }), NullLogger<ContentHashValidationMiddleware>.Instance);

            var bodyBuffer = Encoding.UTF8.GetBytes(new string('-', RequestBodyByteSize));
            var hash = SHA256.Create().ComputeHash(bodyBuffer);
            var hashHeader = BitConverter.ToString(hash).Replace("-", "");

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(bodyBuffer);
            _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashHeader;
            _httpContext.Request.Method = "POST";
            _httpContext.SetEndpoint(new Endpoint(_ => Task.CompletedTask,
                                                       new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                       "someRoute"));
        }

        [Params(8, 2048, 4096, 16384)]
        public int RequestBodyByteSize;

        [ParamsAllValues]
        public PoolKind PoolKind;

        [Benchmark]
        public async Task Run()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}