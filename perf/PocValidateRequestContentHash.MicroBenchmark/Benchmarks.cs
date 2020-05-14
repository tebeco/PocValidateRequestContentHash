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
        private DefaultHttpContext _smallHttpContextNotValidated;
        private DefaultHttpContext _bigHttpContextNotValidated;
        private DefaultHttpContext _smallHttpContextValidated;
        private DefaultHttpContext _bigHttpContextValidated;
        private ContentHashValidationMiddleware _middleware;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var next = new RequestDelegate(_ => Task.CompletedTask);
            _middleware = new ContentHashValidationMiddleware(next, Options.Create(new ContentHashValidationOptions()));

            _smallHttpContextNotValidated = new DefaultHttpContext();
            _smallHttpContextNotValidated.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            _smallHttpContextNotValidated.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = "239f59ed55e737c77147cf55ad0c1b030b6d7ee748a7426952f9b852d5a935e5";
            _smallHttpContextNotValidated.Request.Method = "POST";

            _smallHttpContextValidated = new DefaultHttpContext();
            _smallHttpContextValidated.Request.Path = "/validationcontent/validated";
            _smallHttpContextValidated.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
            _smallHttpContextValidated.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = "239f59ed55e737c77147cf55ad0c1b030b6d7ee748a7426952f9b852d5a935e5";
            _smallHttpContextValidated.Request.Method = "POST";

            var buffer = Encoding.UTF8.GetBytes(new string('-', 16384));
            var hash = SHA256.Create().ComputeHash(buffer);
            var hashStringified = BitConverter.ToString(hash).Replace("-", "");

            _bigHttpContextNotValidated = new DefaultHttpContext();
            _bigHttpContextNotValidated.Request.Body = new MemoryStream(buffer); //16Ko
            _bigHttpContextNotValidated.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            _bigHttpContextNotValidated.Request.Method = "POST";

            _bigHttpContextValidated = new DefaultHttpContext();
            _smallHttpContextValidated.Request.Path = "/validationcontent/validated";
            _bigHttpContextValidated.Request.Body = new MemoryStream(buffer); //16Ko
            _bigHttpContextValidated.Request.Headers[ContentHashValidationOptions.DefaultHeaderName] = hashStringified;
            _bigHttpContextValidated.Request.Method = "POST";
        }

        [Benchmark]
        public async Task Not_Validated_Small_Payload()
        {
            await _middleware.InvokeAsync(_smallHttpContextNotValidated);
        }

        [Benchmark]
        public async Task Not_Validated_16KB_Payload()
        {
            await _middleware.InvokeAsync(_smallHttpContextNotValidated);
        }

        [Benchmark]
        public async Task Validated_Small_Payload()
        {
            await _middleware.InvokeAsync(_smallHttpContextNotValidated);
        }

        [Benchmark]
        public async Task Validated_16KB_Payload()
        {
            await _middleware.InvokeAsync(_smallHttpContextNotValidated);
        }
    }
}
