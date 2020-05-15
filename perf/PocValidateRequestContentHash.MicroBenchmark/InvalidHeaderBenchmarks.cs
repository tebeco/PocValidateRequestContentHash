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
    public class InvalidHeaderBenchmarks
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
            var hashHeader = "00";
            if (!FastCorruptedHeader)
            {
                hashHeader = BitConverter.ToString(hash);
                hashHeader = hashHeader[0..^2] + '0';
            }

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(bodyBuffer);
            if (WithHeader)
            {
                _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashHeader;
            }
            _httpContext.Request.Method = "POST";
            _httpContext.SetEndpoint(new Endpoint(_ => Task.CompletedTask,
                                                     new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                     "someRoute"));
        }

        [Params(8, 4096, 16384)]
        public int RequestBodyByteSize;

        [Params(true, false)]
        public bool WithHeader;

        [Params(true, false)]
        public bool FastCorruptedHeader;

        [Benchmark]
        public async Task Validate()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}