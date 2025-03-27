using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace task_management.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LogOperationAttribute : ActionFilterAttribute
    {
        private Stopwatch _timer;
        private string _operationName;
        private ILogger _logger;

        public LogOperationAttribute(string operationName = null)
        {
            _operationName = operationName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = Stopwatch.StartNew();
            string controller = context.RouteData.Values["controller"]?.ToString();
            string action = context.RouteData.Values["action"]?.ToString();

            // Identificar qué servicios están siendo utilizados
            var services = IdentifyServicesUsed(context);
            string serviceNames = string.Join(", ", services);

            _operationName ??= $"{controller}.{action} [{serviceNames}]";

            // Obtener logger del DI
            _logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("Operations");

            _logger.LogInformation($"🚀 INICIANDO: {_operationName} - {DateTime.Now:HH:mm:ss}");
            _logger.LogInformation($"⚙️ EN PROCESO: {_operationName} con parámetros: {string.Join(", ", context.ActionArguments.Keys)}");

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            if (context.Exception != null)
            {
                _logger.LogError($"❌ ERROR: {_operationName} - {context.Exception.Message}");
            }
            else
            {
                _logger.LogInformation($"✅ COMPLETADO: {_operationName} en {_timer.ElapsedMilliseconds}ms");
            }
            _logger.LogInformation($"🛑 TERMINADO: {_operationName} - {DateTime.Now:HH:mm:ss}");

            base.OnActionExecuted(context);
        }

        private string[] IdentifyServicesUsed(ActionExecutingContext context)
        {
            try
            {
                // Obtener el controlador
                var controller = context.Controller;

                // Obtener las propiedades del controlador que son servicios inyectados
                var serviceProperties = controller.GetType().GetProperties()
                    .Where(p => p.PropertyType.Name.Contains("Service") ||
                                p.PropertyType.Name.Contains("Repository") ||
                                p.PropertyType.GetInterfaces().Any(i =>
                                    i.Name.Contains("Service") || i.Name.Contains("Repository")))
                    .ToList();

                // Extraer nombres de servicios
                return serviceProperties
                    .Select(p => p.PropertyType.Name.Replace("Service", "").Replace("Repository", "").Replace("I", ""))
                    .ToArray();
            }
            catch
            {
                return new[] { "Unknown" };
            }
        }
    }
}
