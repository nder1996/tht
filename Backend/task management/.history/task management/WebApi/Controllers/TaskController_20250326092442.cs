using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task_management.Domain.Interfaces;

namespace task_management.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> Task()
        {
            var response = await _taskService.GetTasks();

            return StatusCode(response.Meta?.StatusCode ?? 200, response);
        }

        [HttpGet("prueba")]
        public IActionResult Prueba()
        {
            return Ok("Hola Mundo");
        }
    }
}
