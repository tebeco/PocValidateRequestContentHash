using System;
using System.Buffers;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MyWebApi.ContentHashValidation
{
    public class ContentHashValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ContentHashValidationOptions _options;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly ArrayPool<byte> _hashArrayPool;

        public ContentHashValidationMiddleware(RequestDelegate next, IOptions<ContentHashValidationOptions> options)
        {
            _next = next;
            _options = options.Value;
            _hashAlgorithm = HashAlgorithm.Create(_options.HashName);

            _hashArrayPool = _options.PoolKind switch
            {
                PoolKind.SharedArrayPool => ArrayPool<byte>.Shared,
                PoolKind.ArrayPool => ArrayPool<byte>.Create(_hashAlgorithm.HashSize / 8, 512),
                PoolKind.FixedLengthLockFree => new FixedLengthLockFreeArrayPool<byte>(512),
                PoolKind.FixedLengthWithLock => new FixedLengthWithLockArrayPool<byte>(512),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata?.GetMetadata<IContentHashValidationMetadata>() != null)
            {
                if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var expectedHash)
                    || expectedHash[0].Length << 2 != _hashAlgorithm.HashSize)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                var readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                while (!readResult.IsCompleted && !readResult.IsCanceled)
                {
                    readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                    context.Request.BodyReader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                }

                if (context.RequestAborted.IsCancellationRequested)
                {
                    return;
                }

                var validationResult = ContentHashValidationResult.Failure;
                var requestHashBuffer = _hashArrayPool.Rent(_hashAlgorithm.HashSize / 8);

                bool gotHash;
                if (!readResult.Buffer.IsSingleSegment)
                {
                    var requestBuffer = ArrayPool<byte>.Shared.Rent((int)readResult.Buffer.Length);
                    readResult.Buffer.CopyTo(requestBuffer);
                    gotHash = TryGetRequestHash(requestBuffer, requestHashBuffer, out _);
                    ArrayPool<byte>.Shared.Return(requestBuffer);
                }
                else
                {
                    gotHash = TryGetRequestHash(readResult.Buffer.FirstSpan, requestHashBuffer, out _);
                }

                if (gotHash)
                {
                    validationResult = CompareHash(expectedHash, requestHashBuffer)
                        ? ContentHashValidationResult.Success
                        : ContentHashValidationResult.Failure;
                }
                _hashArrayPool.Return(requestHashBuffer);

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

        private bool TryGetRequestHash(ReadOnlySpan<byte> requestBuffer, byte[] requestHashBuffer, out int hashSize) =>
            _hashAlgorithm.TryComputeHash(requestBuffer, requestHashBuffer, out hashSize);
    }
}
