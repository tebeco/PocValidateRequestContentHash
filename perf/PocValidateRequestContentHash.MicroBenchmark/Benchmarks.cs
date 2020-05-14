using System;
using System.IO;
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
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            _httpContext.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = "239f59ed55e737c77147cf55ad0c1b030b6d7ee748a7426952f9b852d5a935e5";
            _httpContext.Request.Method = "POST";
        }

        [Benchmark]
        public async Task Scenario1Async()
        {
            await _middleware.InvokeAsync(_httpContext);
        }
    }
}
