using task_management.Application.Service;
using task_management.Domain.Exceptions;

namespace task_management.WebApi.Middleware
{
    public class MethodNotAllowedMiddleware
    {
        private readonly RequestDelegate _next;

        public MethodNotAllowedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 405)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 405;

                var errorMessage = $"El método {context.Request.Method} no está permitido para la ruta {context.Request.Path}";
                var response = ResponseApiBuilderService.Failure<object>(
                    errorCode: "METHOD_NOT_ALLOWED",
                    errorDescription: errorMessage,
                    statusCode: 405
                );

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
