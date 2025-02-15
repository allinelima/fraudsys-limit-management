using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Middlewares
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
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() 
                                ?? Guid.NewGuid().ToString();

            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Correlation-ID"] = correlationId;
                return Task.CompletedTask;
            });

            context.Items["CorrelationId"] = correlationId;  // Adiciona ao contexto para acesso posterior
            await _next(context);
        }
    }
}