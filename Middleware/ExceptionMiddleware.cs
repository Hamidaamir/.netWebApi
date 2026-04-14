using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                var problem = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Detail = ex.Message,
                    Status = 500,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(problem));
            }
        }
    }
}
