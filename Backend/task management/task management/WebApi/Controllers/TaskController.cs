using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task_management.Domain.Interfaces;
using task_management.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using task_management.Application.Dtos.Request;

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

        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _taskService.GetTaskById(id);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _taskService.DeleteTask(id);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskRequest taskRequest)
        {

            var response = await _taskService.CreateTask(taskRequest);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskRequest taskRequest)
        {
            var response = await _taskService.UpdateTask(id,taskRequest);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }



        [HttpGet("debug-table")]
        public async Task<IActionResult> DebugTable()
        {
            try
            {
                await using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                var sql = @"
            SELECT column_name, data_type, character_maximum_length
            FROM information_schema.columns
            WHERE table_name = 'tasks'
            ORDER BY ordinal_position;";

                await _context.Database.OpenConnectionAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = sql;

                var result = new List<object>();

                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.Add(new
                    {
                        ColumnName = reader.GetString(0),
                        DataType = reader.GetString(1),
                        MaxLength = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2)
                    });
                }

                return Ok(new { TableStructure = result });
            }
            catch (NpgsqlException ex)
            {
               // _logger.LogError(ex, "Database error while retrieving table structure");
                return StatusCode(500, new { Error = "Database operation failed", Details = ex.Message });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Unexpected error in DebugTable method");
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }

        [HttpGet("prueba")]
        public IActionResult Prueba()
        {
            return Ok("Hola Mundo");
        }
    }
}
