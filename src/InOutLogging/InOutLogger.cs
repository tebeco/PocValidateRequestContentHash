using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InOutLogging
{
    public static class InOutLogger
    {
        private static Action<ILogger, InOutLoggingWay, string, PathString, string, Exception> _incomingRequest=
            LoggerMessage.Define<InOutLoggingWay, string, PathString, string>(LogLevel.Information, 0, "{way}: '{method}' - '{uri}' - '{content}'");

        private static Action<ILogger, InOutLoggingWay, string, PathString, Exception> _incomingRequestNoContent =
            LoggerMessage.Define<InOutLoggingWay, string, PathString>(LogLevel.Information, 1, "{way}: '{method}' - '{uri}'");

        private static Action<ILogger, InOutLoggingWay, string, PathString, int, long, string, Exception> _outgoingResponse =
            LoggerMessage.Define<InOutLoggingWay, string, PathString, int, long, string>(LogLevel.Information, 10, "{way}: '{method}' - '{uri}' - {statusCode} - in {delay} ms - '{content}'");

        private static Action<ILogger, InOutLoggingWay, string, PathString, int, long, Exception> _outgoingResponseNoContent =
            LoggerMessage.Define<InOutLoggingWay, string, PathString, int, long>(LogLevel.Information, 11, "{way}: '{method}' - '{uri}' - {statusCode} - in {delay} ms");


        public static void IncomingRequest(this ILogger logger, string method, string path, string content) =>
            _incomingRequest(logger, InOutLoggingWay.OutgoingResponse, method, path, content, null);

        public static void IncomingRequest(this ILogger logger, string method, string path) =>
            _incomingRequestNoContent(logger, InOutLoggingWay.OutgoingResponse, method, path, null);

        public static void OutgoingResponseRequest(this ILogger logger, string method, string path, int statusCode, long delay, string content) =>
            _outgoingResponse(logger, InOutLoggingWay.OutgoingResponse, method, path, statusCode, delay, content, null);

        public static void OutgoingResponseRequest(this ILogger logger, string method, string path, int statusCode, long delay) =>
            _outgoingResponseNoContent(logger, InOutLoggingWay.OutgoingResponse, method, path, statusCode, delay, null);
    }
}
