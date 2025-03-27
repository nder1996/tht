using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using task_management.Application.Dtos.Response;
using task_management.Application.Service;
using task_management.Domain.Exceptions;

namespace task_management.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = exception switch
            {
                ApiException apiEx => CreateApiExceptionResponse(apiEx),
                _ => CreateDefaultExceptionResponse(exception)
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Meta?.StatusCode ?? 500;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response, jsonOptions));
        }

        private static ApiResponse<object> CreateApiExceptionResponse(ApiException exception)
        {
            return ResponseApiBuilderService.Failure<object>(
                exception.ErrorCode,
                exception.Message,
                exception.StatusCode);
        }


        private static ApiResponse<object> CreateDefaultExceptionResponse(Exception exception)
        {
            string message;

            if (exception is DbUpdateException dbEx)
            {
                // Extraer el mensaje de la excepción interna
                message = dbEx.InnerException?.Message ??
                         "Error al guardar en la base de datos. Verifique los datos proporcionados.";
            }
            else
            {
                message = $"Ocurrió un error: {exception.Message}";
            }

            return ResponseApiBuilderService.Failure<object>(
                "ERROR_INTERNO",
                message,
                500);
        }

    }
}
