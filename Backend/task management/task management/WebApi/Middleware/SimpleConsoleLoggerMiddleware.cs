namespace task_management.WebApi.Middleware
{
    public class SimpleConsoleLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleConsoleLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString().Substring(0, 8);
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] INICIO [{requestId}] {requestMethod} {requestPath}");
            Console.ResetColor();

            try
            {
                await _next(context);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] FIN [{requestId}] {context.Response.StatusCode}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERROR [{requestId}] {ex.Message}");
                Console.ResetColor();

                throw;
            }
        }
    }

    public static class SimpleLoggingExtensions
    {
        public static IApplicationBuilder UseSimpleConsoleLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SimpleConsoleLoggerMiddleware>();
        }
    }
}
