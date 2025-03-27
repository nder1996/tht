using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using task_management.Application.Dtos.Request;
using task_management.Application.Service;
using task_management.Domain.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskRequest taskRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                    
                // Convertir los errores de validación en un mensaje más amigable
                string errorMessage = string.Join(", ", errors);
                
                // Identificar el primer campo con error para un mensaje más específico
                string errorField = "";
                foreach (var key in ModelState.Keys)
                {
                    if (ModelState[key].Errors.Count > 0)
                    {
                        errorField = key;
                        break;
                    }
                }
                
                string errorCode = !string.IsNullOrEmpty(errorField) 
                    ? $"ERROR_EN_{errorField.ToUpper()}" 
                    : "VALIDATION_ERROR";
                
                return BadRequest(ResponseApiBuilderService.Failure<object>(
                    errorCode: errorCode,
                    errorDescription: errorMessage,
                    statusCode: 400
                ));
            }

            var response = await _taskService.CreateTask(taskRequest);
            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }
    }
} 