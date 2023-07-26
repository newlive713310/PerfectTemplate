using PerfectTemplate.Domain.Helpers;

namespace PerfectTemplate.WebApi.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["X-Correlation-Id"].ToString();

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers["X-Correlation-Id"] = correlationId;
            }

            context.TraceIdentifier = correlationId;

            CorrelationIdContext.SetCorrelationId(correlationId);
            Serilog.Context.LogContext.PushProperty("CorrelationID", correlationId);

            await _next(context);
        }
    }
}