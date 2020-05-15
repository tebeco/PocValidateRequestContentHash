using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyWebApi.ContentHashValidation;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private DefaultHttpContext _httpContext;
        private ContentHashValidationMiddleware _middleware;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var next = new RequestDelegate(_ => Task.CompletedTask);
            _middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            var bodyBuffer = Encoding.UTF8.GetBytes(new string('-', RequestBodyByteSize));
            var hash = SHA256.Create().ComputeHash(bodyBuffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(bodyBuffer);
            _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            _httpContext.Request.Method = "POST";
            if (RequireContentValidation)
            {
                _httpContext.SetEndpoint(new Endpoint(_ => Task.CompletedTask,
                                                         new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                         "someRoute"));
            }
        }

        [Params(8, 2048, 4096, 16384)]
        public int RequestBodyByteSize;

        [Params(true, false)]
        public bool RequireContentValidation;

        [Benchmark]
        public async Task Not_Validated_Small_Payload()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}