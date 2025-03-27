using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using task_management.Application.Service;
using task_management.Domain.Exceptions;

namespace task_management.WebApi.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
               // _logger.LogWarning(ex, "Error de validación");
                await HandleExceptionAsync(context, ex, 400);
            }
            // Otros catch...
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Usar tu servicio de respuesta personalizado
            var response = ResponseApiBuilderService.Failure<object>(
                errorCode: "VALIDATION_ERROR",
                errorDescription: exception.Message,
                statusCode: statusCode
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    // También necesitas configurar un filtro personalizado para validaciones de modelo
    public static class ConfigureModelValidationExtensions
    {
        public static IServiceCollection ConfigureModelValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var response = ResponseApiBuilderService.Failure<object>(
                        errorCode: "VALIDATION_ERROR",
                        errorDescription: GetValidationErrorMessage(context.ModelState),
                        statusCode: 400
                    );

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        private static string GetValidationErrorMessage(ModelStateDictionary modelState)
        {
            // Extrae los mensajes de error
            var errors = modelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => $"{e.Key}: {string.Join(", ", e.Value.Errors.Select(err => err.ErrorMessage))}")
                .ToList();

            return string.Join("; ", errors);
        }
    }
}
