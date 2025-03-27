using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using task_management.Application.Service;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;
using task_management.Infrastructure.Persistence.Repository;
using task_management.WebApi.Extensions;
using task_management.WebApi.Filters;
using task_management.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => {
    config
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(
            path: "logs/taskmanager-.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] " +
                "#{if Level='Fatal'}🔥{else if Level='Error'}❌{else if Level='Warning'}⚠️" +
                "{else if Level='Information'}ℹ️{else}✅{end}# {Message:lj}{NewLine}{Exception}")
        .Enrich.FromLogContext();
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Configuraci�n de PostgreSQL
builder.Services.AddDbContext<TaskDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
       .EnableSensitiveDataLogging()
       .LogTo(Console.WriteLine, LogLevel.Information));
// Registrar servicios
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
  //  options.Filters.Add(new LogOperationAttribute());

});




builder.Services.ConfigureModelValidation();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging(options => {
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
    options.GetLevel = (httpContext, elapsed, ex) =>
        httpContext.Response.StatusCode >= 500 ?
            Serilog.Events.LogEventLevel.Error : Serilog.Events.LogEventLevel.Information;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares para manejo de errores deben ir primero
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseGlobalExceptionHandler();
app.UseMiddleware<MethodNotAllowedMiddleware>();
app.UseSimpleConsoleLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
