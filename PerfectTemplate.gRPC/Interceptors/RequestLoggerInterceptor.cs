using Grpc.Core;
using Grpc.Core.Interceptors;
using PerfectTemplate.Domain.Helpers;
using Serilog;
using System.Diagnostics;

namespace PerfectTemplate.gRPC.Interceptors
{
    public class RequestLoggerInterceptor : Interceptor
    {
        private const string MessageTemplate =
          "{RequestMethod} responded {StatusCode} in {Elapsed:0.0000} ms";

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
          ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var sw = Stopwatch.StartNew();

            var correlationId = context.RequestHeaders
                  .FirstOrDefault(h => h.Key.Equals("X-Correlation-Id", StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrWhiteSpace(correlationId))
                correlationId = Guid.NewGuid().ToString();
            CorrelationIdContext.SetCorrelationId(correlationId);
            using (Serilog.Context.LogContext.PushProperty("CorrelationID", correlationId))
            {
                var response = await base.UnaryServerHandler(request, context, continuation);

                sw.Stop();
                Log.Logger.Information(MessageTemplate,
                      context.Method,
                      context.Status.StatusCode,
                      sw.Elapsed.TotalMilliseconds);

                return response;
            }
        }
    }
}