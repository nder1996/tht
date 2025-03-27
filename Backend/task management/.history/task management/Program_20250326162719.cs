using Microsoft.EntityFrameworkCore;
using task_management.Application.Service;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;
using task_management.Infrastructure.Persistence.Repository;
using task_management.WebApi.Extensions;
using task_management.WebApi.Filters;
using task_management.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);


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
    options.Filters.Add(new LogOperationAttribute());

});




builder.Services.ConfigureModelValidation();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares para manejo de errores deben ir primero
app.UseMiddleware<ValidationExceptionMiddleware>(); // Primero para validaciones espec�ficas
app.UseGlobalExceptionHandler();
app.UseMiddleware<MethodNotAllowedMiddleware>();
app.UseSimpleConsoleLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
