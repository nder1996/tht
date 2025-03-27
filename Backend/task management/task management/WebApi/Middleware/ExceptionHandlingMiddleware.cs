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
            string errorCode = "ERROR_INTERNO";

            if (exception is DbUpdateException dbEx)
            {
                // Extraer el mensaje de la excepción interna de manera más específica
                var innerException = dbEx.InnerException;
                
                // Si hay una excepción interna, intentar extraer información más específica
                if (innerException != null)
                {
                    // Para excepciones de PostgreSQL (Npgsql)
                    if (innerException.GetType().Name.Contains("Npgsql"))
                    {
                        // Intentar extraer el campo específico que causó el error
                        var errorMessage = innerException.Message;
                        
                        // Buscar campo específico en el mensaje
                        if (errorMessage.Contains("violates"))
                        {
                            if (errorMessage.Contains("not-null constraint"))
                            {
                                // Error de campo requerido
                                var fieldMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"column ""([^""]+)""");
                                if (fieldMatch.Success)
                                {
                                    string fieldName = fieldMatch.Groups[1].Value;
                                    message = $"El campo '{fieldName}' es obligatorio.";
                                    errorCode = "CAMPO_REQUERIDO";
                                    return ResponseApiBuilderService.Failure<object>(errorCode, message, 400);
                                }
                            }
                            else if (errorMessage.Contains("unique constraint"))
                            {
                                // Error de duplicidad
                                var constraintMatch = System.Text.RegularExpressions.Regex.Match(errorMessage, @"constraint ""([^""]+)""");
                                if (constraintMatch.Success)
                                {
                                    string constraintName = constraintMatch.Groups[1].Value;
                                    message = $"Ya existe un registro con los mismos valores ({constraintName}).";
                                    errorCode = "DUPLICIDAD";
                                    return ResponseApiBuilderService.Failure<object>(errorCode, message, 400);
                                }
                            }
                            else if (errorMessage.Contains("foreign key constraint"))
                            {
                                // Error de clave foránea
                                message = "No se puede crear/actualizar el registro porque hace referencia a un registro que no existe.";
                                errorCode = "REFERENCIA_INVALIDA";
                                return ResponseApiBuilderService.Failure<object>(errorCode, message, 400);
                            }
                        }
                        
                        // Si no pudimos identificar un error específico, usar el mensaje original
                        message = errorMessage;
                        errorCode = "ERROR_BASE_DATOS";
                        return ResponseApiBuilderService.Failure<object>(errorCode, message, 400);
                    }
                    else
                    {
                        message = innerException.Message;
                    }
                }
                else
                {
                    message = "Error al guardar en la base de datos. Verifique los datos proporcionados.";
                }
            }
            else
            {
                message = $"Ocurrió un error: {exception.Message}";
            }

            return ResponseApiBuilderService.Failure<object>(
                errorCode,
                message,
                500);
        }

    }
}
