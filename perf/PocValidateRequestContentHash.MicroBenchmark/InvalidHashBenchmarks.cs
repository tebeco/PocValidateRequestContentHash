using System.IO;
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
    public class InvalidHashBenchmarks
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
            var hashHeader = new string('0', 64);

            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(bodyBuffer);
            _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashHeader;
            _httpContext.Request.Method = "POST";
            _httpContext.SetEndpoint(new Endpoint(_ => Task.CompletedTask,
                                                     new EndpointMetadataCollection(new ValidateContentHashAttribute()),
                                                     "someRoute"));
        }

        [Params(8, 2048, 3072, 4096 - 64, 4096, 16384)]
        public int RequestBodyByteSize;

        [Benchmark]
        public async Task Validate()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}