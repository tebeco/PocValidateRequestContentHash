using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ContentHashValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace PocValidateRequestContentHash.MicroBenchmark
{
    [MemoryDiagnoser]
    public class ValidDataBenchmarks
    {
#nullable disable
        private DefaultHttpContext _httpContext;
        private ContentHashValidationMiddleware _middleware;
#nullable enable

        [GlobalSetup]
        public void GlobalSetup()
        {
            var next = new RequestDelegate(_ => Task.CompletedTask);
            _middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()), NullLogger<ContentHashValidationMiddleware>.Instance);

            var bodyBuffer = Encoding.UTF8.GetBytes(new string('-', RequestBodyByteSize));
            var hash = SHA256.Create().ComputeHash(bodyBuffer);
            var hashHeader = BitConverter.ToString(hash).Replace("-", "");

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(bodyBuffer);
            _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashHeader;
            _httpContext.Request.Method = "POST";
            if (WithValidation)
            {
                _httpContext.SetEndpoint(new Endpoint(_ => Task.CompletedTask,
                                                         new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                         "someRoute"));
            }
        }

        [Params(8, 2048, 3072, 4096 - 64, 4096, 16384)]
        public int RequestBodyByteSize;

        [Params(true, false)]
        public bool WithValidation;

        [Benchmark]
        public async Task RunValidData()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}