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
                var readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                while (!readResult.IsCompleted && !readResult.IsCanceled)
                {
                    readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                }

                var hashBytes = readResult.Buffer.IsSingleSegment
                            ? GetRequestHash(readResult.Buffer.FirstSpan, out var _)
                            : GetRequestHash(readResult.Buffer.ToArray(), out var _);

                var expectedHash = context.Request.Headers[_options.HeaderName][0]; //can throw

                var validationResult = CompareHash(expectedHash, hashBytes);
                ArrayPool<byte>.Shared.Return(hashBytes);

                if (!validationResult.Succeed)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.BodyWriter.CompleteAsync();
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private ContentHashValidationResult CompareHash(string expectedHash, byte[] hashedContent)
        {
            var expected = expectedHash.AsSpan();
            for (int i = 0; i < hashedContent.Length; i++)
            {
                if (!int.TryParse(expected.Slice(i * 2, 2), NumberStyles.AllowHexSpecifier, null, out var num) || num != hashedContent[i])
                    return ContentHashValidationResult.Failure;
            }
            return ContentHashValidationResult.Success;
        }

        private byte[] GetRequestHash(ReadOnlySpan<byte> requestBuffer, out int hashSize)
        {
            var requestHashBuffer = ArrayPool<byte>.Shared.Rent(_hashAlgorithm.HashSize / 8);

            _ = _hashAlgorithm.TryComputeHash(requestBuffer, requestHashBuffer, out hashSize);

            return requestHashBuffer;
        }
    }
}
