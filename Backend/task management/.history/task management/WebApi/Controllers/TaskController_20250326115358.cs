using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace task_management.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly TaskDbContext _context;

        public TaskController(ITaskService taskService, TaskDbContext context)
        {
            _taskService = taskService;
            _context = context;
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> Task()
        {
            var response = await _taskService.GetTasks();
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }

        [HttpGet("debug-table")]
        public async Task<IActionResult> DebugTable()
        {
            try
            {
                // Obtener la conexión del contexto
                var connection = _context.Database.GetDbConnection() as NpgsqlConnection;
                
                // Consulta para obtener la estructura de la tabla
                var sql = @"
                    SELECT column_name, data_type, character_maximum_length
                    FROM information_schema.columns
                    WHERE table_name = 'tasks'
                    ORDER BY ordinal_position;";

                await _context.Database.OpenConnectionAsync();
                
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                
                var result = new List<object>();
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    result.Add(new
                    {
                        ColumnName = reader.GetString(0),
                        DataType = reader.GetString(1),
                        MaxLength = reader.IsDBNull(2) ? null : reader.GetInt32(2)
                    });
                }

                return Ok(new { TableStructure = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        [HttpGet("prueba")]
        public IActionResult Prueba()
        {
            return Ok("Hola Mundo");
        }
    }
}
