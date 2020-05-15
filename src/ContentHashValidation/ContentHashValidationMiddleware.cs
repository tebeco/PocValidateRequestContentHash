using System;
using System.Buffers;
using System.Globalization;
using System.IO.Pipelines;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MyWebApi.ContentHashValidation
{
    public class ContentHashValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ContentHashValidationOptions _options;
        private readonly HashAlgorithm _hashAlgorithm;

        public ContentHashValidationMiddleware(RequestDelegate next, IOptions<ContentHashValidationOptions> options)
        {
            _next = next;
            _options = options.Value;
            _hashAlgorithm = HashAlgorithm.Create(_options.HashName);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata?.GetMetadata<IContentHashValidationMetadata>() != null)
            {
                if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var expectedHash))
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                var readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                while (!readResult.IsCompleted && !readResult.IsCanceled)
                {
                    readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                }

                if (context.RequestAborted.IsCancellationRequested)
                {
                    return;
                }

                context.Request.BodyReader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);

                var validationResult = ContentHashValidationResult.Failure;
                var requestHashBuffer = ArrayPool<byte>.Shared.Rent(_hashAlgorithm.HashSize / 8);

                if (readResult.Buffer.IsSingleSegment
                            ? GetRequestHash(readResult.Buffer.FirstSpan, requestHashBuffer, out var _)
                            : GetRequestHash(readResult.Buffer.ToArray(), requestHashBuffer, out var _))
                {
                    validationResult = CompareHash(expectedHash, requestHashBuffer)
                        ? ContentHashValidationResult.Success
                        : ContentHashValidationResult.Failure;
                }
                ArrayPool<byte>.Shared.Return(requestHashBuffer);

                if (!validationResult.Succeed)
                {
                    context.Response.StatusCode = 400;
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private bool CompareHash(string expectedHash, byte[] hashedContent)
        {
            var expected = expectedHash.AsSpan();
            for (int i = 0; i < hashedContent.Length; i++)
            {
                if (!int.TryParse(expected.Slice(i * 2, 2), NumberStyles.AllowHexSpecifier, null, out var num) || num != hashedContent[i])
                    return false;
            }
            return true;
        }

        private bool GetRequestHash(ReadOnlySpan<byte> requestBuffer, byte[] requestHashBuffer, out int hashSize) =>
            _hashAlgorithm.TryComputeHash(requestBuffer, requestHashBuffer, out hashSize);
    }
}
