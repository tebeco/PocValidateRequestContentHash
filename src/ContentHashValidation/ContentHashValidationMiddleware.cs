﻿using System.Buffers;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContentHashValidation
{
    public class ContentHashValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ContentHashValidationMiddleware> _logger;
        private readonly ContentHashValidationOptions _options;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly ArrayPool<byte> _hashArrayPool;

        public ContentHashValidationMiddleware(RequestDelegate next, IOptions<ContentHashValidationOptions> options, ILogger<ContentHashValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
            _hashAlgorithm  = (HashAlgorithm)CryptoConfig.CreateFromName(_options.HashName)!;

            _hashArrayPool = _options.PoolKind switch
            {
                PoolKind.SharedArrayPool => ArrayPool<byte>.Shared,
                PoolKind.DedicatedArrayPool => ArrayPool<byte>.Create(_hashAlgorithm.HashSize / 8, 512),
                PoolKind.FixedLengthLockFree => new FixedLengthLockFreeArrayPool<byte>(_hashAlgorithm.HashSize / 8),
                PoolKind.FixedLengthWithLock => new FixedLengthWithLockArrayPool<byte>(_hashAlgorithm.HashSize / 8),
                PoolKind.PreAllocatedFixedLengthLockFree => new FixedLengthLockFreeArrayPool<byte>(_hashAlgorithm.HashSize / 8, 512),
                PoolKind.PreAllocatedFixedLengthWithLock => new FixedLengthWithLockArrayPool<byte>(_hashAlgorithm.HashSize / 8, 512),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata?.GetMetadata<IContentHashValidationMetadata>() != null)
            {
                if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var headerHashValue))
                {
                    _logger.LogWarning("Missing header {HeaderName}", _options.HeaderName);
                    context.Response.StatusCode = 400;
                    return;
                }

                if (headerHashValue.Count != 1)
                {
                    _logger.LogWarning("Corruputed header: {HeaderName} should only have a single value. Current value: {HeaderValue}", _options.HeaderName, headerHashValue.ToString());
                    context.Response.StatusCode = 400;
                    return;
                }

                var requestHeaderHash = headerHashValue.ToString();
                if (requestHeaderHash.Length << 2 != _hashAlgorithm.HashSize)
                {
                    _logger.LogWarning("Corruputed header: {HeaderName} with value {HeaderValue}", _options.HeaderName, requestHeaderHash);
                    context.Response.StatusCode = 400;
                    return;
                }

                var readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                context.Request.BodyReader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                while (!readResult.IsCompleted && !readResult.IsCanceled)
                {
                    readResult = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
                    context.Request.BodyReader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                }

                if (context.RequestAborted.IsCancellationRequested)
                {
                    _logger.LogWarning("CancellationRequested");

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
                    validationResult = CompareHash(requestHeaderHash, requestHashBuffer)
                        ? ContentHashValidationResult.Success
                        : ContentHashValidationResult.Failure;
                }
                _hashArrayPool.Return(requestHashBuffer);

                if (!validationResult.Succeed)
                {
#if !BOMBARDIER_BUILD
// This seems to generate a TONS of missmatch while using bombardier, not sure why yet
                    _logger.LogWarning("Hash missmatch, expected: {expected}", requestHeaderHash);
#endif

                    context.Response.StatusCode = 400;
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private static bool CompareHash(string expectedHash, byte[] hashedContent)
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
